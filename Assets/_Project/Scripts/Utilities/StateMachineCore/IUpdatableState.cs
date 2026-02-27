namespace Assets._Project.Scripts.Utilities.StateMachineCore
{
    public interface IUpdatableState : IState
    {
        void Update(float deltaTime);
    }
}