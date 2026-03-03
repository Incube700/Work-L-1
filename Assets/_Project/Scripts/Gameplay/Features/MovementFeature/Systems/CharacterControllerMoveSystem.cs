using UnityEngine;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;

namespace Assets._Project.Scripts.Gameplay.Features.MovementFeature
{
    public sealed class CharacterControllerMoveSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<Vector3> _moveDirection;
        private ReactiveVariable<float> _moveSpeed;
        private CharacterController _controller;

        public void OnInit(Entity entity)
        {
            _moveDirection = entity.MoveDirection;
            _moveSpeed = entity.MoveSpeed;

            _controller = entity.CharacterController;
        }

        public void OnUpdate(float deltaTime)
        {
            Vector3 dir = _moveDirection.Value;

            if (dir.sqrMagnitude > 1f)
                dir.Normalize();

            _controller.Move(dir * _moveSpeed.Value * deltaTime);
        }
    }
}
