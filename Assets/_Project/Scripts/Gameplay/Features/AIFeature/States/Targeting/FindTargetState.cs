using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Utilities.StateMachineCore;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Targeting
{
    public sealed class FindTargetState : State, IUpdatableState
    {
        private readonly ITargetSelector _selector;
        private readonly EntitiesLifeContext _life;
        private readonly ReactiveVariable<Entity> _currentTarget;
        private readonly Action<float> _updateAction;

        public FindTargetState(ITargetSelector selector, EntitiesLifeContext life, Entity owner, Action<float> updateAction = null)
        {
            _selector = selector;
            _life = life;
            _currentTarget = owner.CurrentTarget;
            _updateAction = updateAction;
        }

        public void Update(float deltaTime)
        {
            _currentTarget.Value = _selector.SelectTargetFrom(_life.Entities);
            _updateAction?.Invoke(deltaTime);
        }
    }
}
