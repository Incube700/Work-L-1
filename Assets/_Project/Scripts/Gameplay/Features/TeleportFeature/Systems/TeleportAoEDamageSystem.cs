using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.TeleportFeature
{
    public sealed class TeleportAoEDamageSystem : IInitializableSystem, IDisposableSystem
    {
        private const int CollidersBufferSize = 64;

        private readonly CollidersRegistryService _collidersRegistryService;
        private readonly Collider[] _collidersBuffer;

        private Entity _self;
        private Transform _transform;

        private float _damage;
        private float _radius;
        private int _mask;

        private SimpleEvent _teleportedEvent;

        public TeleportAoEDamageSystem(CollidersRegistryService collidersRegistryService)
        {
            _collidersRegistryService = collidersRegistryService;
            _collidersBuffer = new Collider[CollidersBufferSize];
        }

        public void OnInit(Entity entity)
        {
            _self = entity;
            _transform = entity.GetComponent<TransformComponent>().Value;

            _damage = entity.GetComponent<TeleportAoEDamage>().Value;
            _radius = entity.GetComponent<TeleportAoERadius>().Value;
            _mask = entity.GetComponent<TeleportAoEMask>().Value;

            _teleportedEvent = entity.GetComponent<TeleportedEvent>().Value;
            _teleportedEvent.Invoked += OnTeleported;
        }

        public void OnDispose()
        {
            if (_teleportedEvent != null)
                _teleportedEvent.Invoked -= OnTeleported;

            _self = null;
            _transform = null;
            _teleportedEvent = null;
        }

        private void OnTeleported()
        {
            if (_collidersRegistryService == null)
                return;

            if (_radius <= 0f)
                return;

            if (_damage <= 0f)
                return;

            // Собираем коллайдеры вокруг точки телепорта.
            int count = Physics.OverlapSphereNonAlloc(
                _transform.position,
                _radius,
                _collidersBuffer,
                _mask,
                QueryTriggerInteraction.Collide);

            Debug.Log($"[AOE] hits={count} dmg={_damage}");

            int sentCount = 0;
            for (int i = 0; i < count; i++)
            {
                Collider collider = _collidersBuffer[i];
                if (collider == null)
                    continue;

                // Превращаем Collider -> Entity через реестр (без GetComponent в цикле).
                if (_collidersRegistryService.TryGetEntity(collider, out Entity target) == false)
                    continue;

                if (target == null)
                    continue;

                if (ReferenceEquals(target, _self))
                    continue;

                // Урон наносим через запрос (данные), а не напрямую в чужой Health.
                if (target.HasComponent<TakeDamageRequest>() == false)
                    continue;

                target.GetComponent<TakeDamageRequest>().Value.Invoke(_damage);

                if (sentCount < 3)
                {
                    string targetName = target.HasComponent<TransformComponent>() 
                        ? target.GetComponent<TransformComponent>().Value.name 
                        : "Unknown";
                    Debug.Log($"[AOE] damage sent -> {targetName}");
                    sentCount++;
                }
            }
        }
    }
}
