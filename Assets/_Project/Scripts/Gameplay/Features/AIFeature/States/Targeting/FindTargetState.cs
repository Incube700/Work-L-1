using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Utilities.StateMachineCore;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Targeting
{
    public sealed class FindTargetState : State, IUpdatableState
    {
        private readonly ITargetSelector _selector;
        private readonly EntitiesLifeContext _life;
        private readonly ReactiveVariable<Entity> _currentTarget;

        public FindTargetState(ITargetSelector selector, EntitiesLifeContext life, Entity owner)
        {
            _selector = selector;
            _life = life;
            _currentTarget = owner.GetComponent<CurrentTarget>().Value;
        }

        public void Update(float deltaTime)
        {
            _currentTarget.Value = _selector.SelectTargetFrom(_life.Entities);
        }
    }
}