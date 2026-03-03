using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;
using Assets._Project.Scripts.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Hero
{
    public sealed class ManualHeroMoveState : State, IUpdatableState
    {
        private readonly IInputService _input;
        private readonly ReactiveVariable<Vector3> _moveDirection;
        private readonly ReactiveVariable<Vector3> _rotationDirection;

        public ManualHeroMoveState(Entity entity, IInputService input)
        {
            _input = input;
            _moveDirection = entity.MoveDirection;
            _rotationDirection = entity.RotationDirection;
        }

        public void Update(float deltaTime)
        {
            Vector3 dir = _input.Direction.normalized;

            _moveDirection.Value = dir;
            _rotationDirection.Value = dir;
        }

        public override void Exit()
        {
            base.Exit();
            _moveDirection.Value = Vector3.zero;
        }
    }
}
