using System.Collections.Generic;

namespace Assets._Project.Scripts.Utilities.StateMachineCore
{
    public sealed class StateNode<TState> where TState : class, IState
    {
        private readonly List<StateTransition<TState>> _transitions = new List<StateTransition<TState>>();

        public StateNode(TState state)
        {
            State = state;
        }
        
        public TState State { get; }
        public IReadOnlyList<StateTransition<TState>> Transitions => _transitions;

        public void AddTransition(StateTransition<TState> transition)
        {
            _transitions.Add(transition);
        }
    }
}