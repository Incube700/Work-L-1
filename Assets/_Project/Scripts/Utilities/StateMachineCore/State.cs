namespace Assets._Project.Scripts.Utilities.StateMachineCore
{
    public abstract class State : IState
    {
        private readonly SimpleEvent _entered = new SimpleEvent();
        private readonly SimpleEvent _exited = new SimpleEvent();

        public IReadOnlySimpleEvent Entered => _entered;
        public IReadOnlySimpleEvent Exited => _exited;

        public virtual void Enter()
        {
            _entered.Invoke();
        }

        public virtual void Exit()
        {
            _exited.Invoke();
        }
    }
}
