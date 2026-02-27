using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Conditions;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.AIFeature;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.TeleportFeature
{
    public sealed class TeleportTowardsTargetSystem : IInitializableSystem, IDisposableSystem
    {
        private readonly ICondition _canTeleport;

        private Entity _entity;
        private Transform _transform;
        private float _teleportRadius;

        private ReactiveVariable<Entity> _currentTarget;

        private SimpleEvent _teleportRequest;
        private SimpleEvent _teleportedEvent;

        public TeleportTowardsTargetSystem(ICondition canTeleport)
        {
            _canTeleport = canTeleport;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;

            _transform = entity.Transform;
            _teleportRadius = entity.TeleportRadius;

            _currentTarget = entity.GetComponent<CurrentTarget>().Value;

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
            _currentTarget = null;
            _teleportRequest = null;
            _teleportedEvent = null;
        }

        private void OnTeleportRequested()
        {
            if (_canTeleport.IsSatisfied(_entity) == false)
                return;

            Entity target = _currentTarget.Value;

            if (target == null)
                return;

            Vector3 from = _transform.position;
            Vector3 to = target.Transform.position;

            Vector3 dir = to - from;
            dir.y = 0f;

            if (dir.sqrMagnitude < 0.0001f)
                return;

            float distance = dir.magnitude;
            float step = distance;

            if (step > _teleportRadius)
                step = _teleportRadius;

            Vector3 offset = dir.normalized * step;

            _transform.position = from + offset;
            _teleportedEvent.Invoke();
        }
    }
}