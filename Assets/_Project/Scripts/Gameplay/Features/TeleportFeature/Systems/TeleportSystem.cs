using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Conditions;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.TeleportFeature
{
    public sealed class TeleportSystem : IInitializableSystem, IDisposableSystem
    {
        private readonly ICondition _canTeleport;

        private Entity _entity;
        private Transform _transform;
        private float _teleportRadius;

        private SimpleEvent _teleportRequest;
        private SimpleEvent _teleportedEvent;

        public TeleportSystem(ICondition canTeleport)
        {
            _canTeleport = canTeleport;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;

            _transform = entity.Transform;
            _teleportRadius = entity.TeleportRadius;

            _teleportRequest = entity.TeleportRequest;
            _teleportedEvent = entity.TeleportedEvent;

            _teleportRequest.Invoked += OnTeleportRequested;
        }

        public void OnDispose()
        {
            if (_teleportRequest != null)
                _teleportRequest.Invoked -= OnTeleportRequested;

            _entity = null;
            _transform = null;
            _teleportRequest = null;
            _teleportedEvent = null;
        }

        private void OnTeleportRequested()
        {
            if (_canTeleport.IsSatisfied(_entity) == false)
                return;

            Vector3 before = _transform.position;

            Vector2 offset2 = Random.insideUnitCircle * _teleportRadius;
            Vector3 offset3 = new Vector3(offset2.x, 0f, offset2.y);

            _transform.position = before + offset3;

            _teleportedEvent.Invoke();
        }
    }
}