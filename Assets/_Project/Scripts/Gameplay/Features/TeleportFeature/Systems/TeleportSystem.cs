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

        private SimpleEvent<Vector3> _teleportRequest;
        private SimpleEvent _teleportedEvent;

        public TeleportSystem(ICondition canTeleport)
        {
            _canTeleport = canTeleport;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;

            _transform = entity.Transform;

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

        private void OnTeleportRequested(Vector3 position)
        {
            if (_canTeleport.IsSatisfied(_entity) == false)
                return;

            _transform.position = position;

            _teleportedEvent.Invoke();
        }
    }
}
