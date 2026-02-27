namespace Assets._Project.Scripts.Utilities.StateMachineCore
{
    public interface IState
    {
        SimpleEvent Entered { get; }
        SimpleEvent Exited { get; }
        
        void Enter();
        void Exit();
    }
}