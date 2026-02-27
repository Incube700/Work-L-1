using UnityEngine;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;

namespace Assets._Project.Scripts.Gameplay.Features.MovementFeature
{
    public sealed class TransformRotationSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<Vector3> _rotationDirection;
        private Transform _transform;

        public void OnInit(Entity entity)
        {
            _rotationDirection = entity.RotationDirection;
            _transform = entity.GetComponent<TransformComponent>().Value;
        }

        public void OnUpdate(float deltaTime)
        {
            Vector3 dir = _rotationDirection.Value;

            if (dir.sqrMagnitude < 0.0001f)
                return;

            _transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }
}