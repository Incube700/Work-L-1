using System;

namespace Assets._Project.Scripts.Utilities.StateMachineCore
{
    public sealed class EmptyState : State, IUpdatableState
    {
        private readonly Action<float> _updateAction;

        public EmptyState(Action<float> updateAction = null)
        {
            _updateAction = updateAction;
        }

        public void Update(float deltaTime)
        {
            _updateAction?.Invoke(deltaTime);
        }
    }
}
