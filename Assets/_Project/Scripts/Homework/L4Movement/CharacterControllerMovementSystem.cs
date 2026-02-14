using UnityEngine;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;

namespace Assets._Project.Scripts.Homework.L4Movement
{
    public sealed class CharacterControllerMovementSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<Vector3> _moveDirection;
        private ReactiveVariable<float> _moveSpeed;
        private CharacterController _controller;

        public void OnInit(Entity entity)
        {
            _moveDirection = entity.MoveDirection;
            _moveSpeed = entity.MoveSpeed;

            _controller = entity.GetComponent<CharacterControllerComponent>().Value;
        }

        public void OnUpdate(float deltaTime)
        {
            Vector3 dir = _moveDirection.Value;

            if (dir.sqrMagnitude > 1f)
                dir.Normalize();

            _controller.Move(dir * _moveSpeed.Value * deltaTime);

            if (dir.sqrMagnitude < 0.0001f)
                return;

            _controller.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }
}