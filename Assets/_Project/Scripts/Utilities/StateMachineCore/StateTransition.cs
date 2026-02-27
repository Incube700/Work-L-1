using Assets._Project.Scripts.Utilities.Conditions;

namespace Assets._Project.Scripts.Utilities.StateMachineCore
{
    public sealed class StateTransition<TState> where TState : class, IState
    {
        public StateTransition(StateNode<TState> toState, ICondition condition)
        {
            ToState = toState;
            Condition = condition;
        }
        
        public StateNode<TState> ToState { get; }
        public ICondition Condition { get; }
    }
}