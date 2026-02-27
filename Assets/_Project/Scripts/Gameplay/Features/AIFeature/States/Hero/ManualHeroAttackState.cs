using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.AttackFeature;
using Assets._Project.Scripts.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Hero
{
    public sealed class ManualHeroAttackState : State, IUpdatableState
    {
        private readonly SimpleEvent _startAttackRequest;
        private readonly ReactiveVariable<Vector3> _moveDirection;

        public ManualHeroAttackState(Entity entity)
        {
            _startAttackRequest = entity.GetComponent<StartAttackRequest>().Value;
            _moveDirection = entity.MoveDirection;
        }

        public override void Enter()
        {
            base.Enter();
            _moveDirection.Value = Vector3.zero;
            _startAttackRequest.Invoke();
        }

        public void Update(float deltaTime)
        {
            _moveDirection.Value = Vector3.zero;
        }
    }
}