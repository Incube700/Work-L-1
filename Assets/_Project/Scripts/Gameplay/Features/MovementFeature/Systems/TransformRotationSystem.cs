using UnityEngine;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;

namespace Assets._Project.Scripts.Gameplay.Features.MovementFeature
{
    public sealed class TransformRotationSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<Vector3> _moveDirection;
        private Transform _transform;

        public void OnInit(Entity entity)
        {
            _moveDirection = entity.MoveDirection;
            _transform = entity.GetComponent<TransformComponent>().Value;
        }

        public void OnUpdate(float deltaTime)
        {
            Vector3 dir = _moveDirection.Value;

            if (dir.sqrMagnitude < 0.0001f)
                return;

            _transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }
}
