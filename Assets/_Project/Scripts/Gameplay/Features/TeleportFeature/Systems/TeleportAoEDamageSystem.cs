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
            _transform = entity.Transform;

            _damage = entity.TeleportAoEDamage;
            _radius = entity.TeleportAoERadius;
            _mask = entity.TeleportAoEMask;

            _teleportedEvent = entity.TeleportedEvent;
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
            if (_radius <= 0f)
                return;

            int count = Physics.OverlapSphereNonAlloc(
                _transform.position,
                _radius,
                _collidersBuffer,
                _mask,
                QueryTriggerInteraction.Collide);

            for (int i = 0; i < count; i++)
            {
                Collider collider = _collidersBuffer[i];
                if (collider == null)
                    continue;

                if (_collidersRegistryService.TryGetEntity(collider, out Entity target) == false)
                    continue;

                if (ReferenceEquals(target, _self))
                    continue;

                if (target.HasComponent<TakeDamageRequest>() == false)
                    continue;

                target.TakeDamageRequest.Invoke(_damage);
            }
        }
    }
}