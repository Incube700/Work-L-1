namespace Assets._Project.Scripts.Utilities.StateMachineCore
{
    public class ParallelState<TState> : State where TState : class, IState
    {
        public ParallelState(params TState[] states)
        {
            States = states;
        }

        protected TState[] States { get; }

        public override void Enter()
        {
            base.Enter();

            foreach (TState state in States)
                state.Enter();
        }

        public override void Exit()
        {
            base.Exit();

            foreach (TState state in States)
                state.Exit();
        }
    }
}