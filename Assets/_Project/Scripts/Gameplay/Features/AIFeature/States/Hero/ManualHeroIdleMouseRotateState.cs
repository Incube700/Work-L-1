using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;
using Assets._Project.Scripts.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Hero
{
    public sealed class ManualHeroIdleMouseRotateState : State, IUpdatableState
    {
        private const float MouseSensitivity = 10f;

        private readonly IInputService _input;
        private readonly ReactiveVariable<Vector3> _moveDirection;
        private readonly ReactiveVariable<Vector3> _rotationDirection;

        public ManualHeroIdleMouseRotateState(Entity entity, IInputService input)
        {
            _input = input;
            _moveDirection = entity.MoveDirection;
            _rotationDirection = entity.RotationDirection;
        }

        public void Update(float deltaTime)
        {
            _moveDirection.Value = Vector3.zero;

            Vector3 dir = _rotationDirection.Value;

            if (dir.sqrMagnitude < 0.0001f)
                dir = Vector3.forward;

            float yaw = _input.MouseX * MouseSensitivity;
            dir = Quaternion.Euler(0f, yaw, 0f) * dir;

            _rotationDirection.Value = dir.normalized;
        }
    }
}