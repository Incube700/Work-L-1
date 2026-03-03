namespace Assets._Project.Scripts.Utilities.StateMachineCore
{
    public interface IState
    {
        IReadOnlySimpleEvent Entered { get; }
        IReadOnlySimpleEvent Exited { get; }
        
        void Enter();
        void Exit();
    }
}
