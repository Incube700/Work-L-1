using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.AttackFeature;
using Assets._Project.Scripts.Utilities.StateMachineCore;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Hero
{
    public sealed class ManualHeroAttackState : State, IUpdatableState
    {
        private readonly SimpleEvent _startAttackRequest;

        public ManualHeroAttackState(Entity entity)
        {
            _startAttackRequest = entity.StartAttackRequest;
        }

        public override void Enter()
        {
            base.Enter();
            _startAttackRequest.Invoke();
        }

        public void Update(float deltaTime)
        {
        }
    }
}
