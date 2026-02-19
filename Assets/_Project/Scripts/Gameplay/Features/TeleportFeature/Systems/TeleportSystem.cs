using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.EnergyFeature;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.TeleportFeature
{
    public sealed class TeleportSystem : IInitializableSystem, IDisposableSystem
    {
        private Transform _transform;

        private ReactiveVariable<float> _currentEnergy;
        private float _energyCost;
        private float _teleportRadius;

        private ReactiveVariable<bool> _isDead;

        private SimpleEvent _teleportRequest;
        private SimpleEvent _teleportedEvent;

        public void OnInit(Entity entity)
        {
            _transform = entity.GetComponent<TransformComponent>().Value;

            _currentEnergy = entity.GetComponent<CurrentEnergy>().Value;
            _energyCost = entity.GetComponent<TeleportEnergyCost>().Value;
            _teleportRadius = entity.GetComponent<TeleportRadius>().Value;

            _isDead = entity.GetComponent<IsDead>().Value;

            _teleportRequest = entity.GetComponent<TeleportRequest>().Value;
            _teleportedEvent = entity.GetComponent<TeleportedEvent>().Value;

            _teleportRequest.Invoked += OnTeleportRequested;
        }

        public void OnDispose()
        {
            if (_teleportRequest != null)
                _teleportRequest.Invoked -= OnTeleportRequested;

            _transform = null;
            _currentEnergy = null;
            _isDead = null;
            _teleportRequest = null;
            _teleportedEvent = null;
        }

        private void OnTeleportRequested()
        {
            Debug.Log("[TP] Teleport requested");

            // 1) Гварды (защита от некорректных состояний).
            if (_isDead.Value)
                return;

            if (_energyCost <= 0f)
                return;

            if (_currentEnergy.Value < _energyCost)
            {
                Debug.Log("[TP] Not enough energy");
                return;
            }

            // 2) Тратим энергию.
            _currentEnergy.Value -= _energyCost;

            // 3) Выбираем случайную точку в радиусе N вокруг текущей позиции.
            Vector2 offset2 = Random.insideUnitCircle * _teleportRadius;
            Vector3 offset3 = new Vector3(offset2.x, 0f, offset2.y);

            Vector3 before = _transform.position;

            // 4) Телепортируемся (движения/поворота нет — только смена позиции).
            _transform.position += offset3;

            Debug.Log($"[TP] Teleported: {before} -> {_transform.position}. Energy left: {_currentEnergy.Value}");

            // 5) Сообщаем подписчикам, что телепорт завершён (например, AoE-урон).
            _teleportedEvent.Invoke();
        }
    }
}
