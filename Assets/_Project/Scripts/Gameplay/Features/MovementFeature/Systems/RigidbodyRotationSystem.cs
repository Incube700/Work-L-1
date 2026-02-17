using UnityEngine;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;

namespace Assets._Project.Scripts.Gameplay.Features.MovementFeature
{
    public sealed class RigidbodyRotationSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<Vector3> _moveDirection;
        private Rigidbody _rigidbody;

        public void OnInit(Entity entity)
        {
            _moveDirection = entity.MoveDirection;
            _rigidbody = entity.Rigidbody;
        }

        public void OnUpdate(float deltaTime)
        {
            Vector3 dir = _moveDirection.Value;

            if (dir.sqrMagnitude < 0.0001f)
                return;

            Quaternion rot = Quaternion.LookRotation(dir.normalized, Vector3.up);
            _rigidbody.MoveRotation(rot);
        }
    }
}