using Assets._Project.Scripts.Utilities.StateMachineCore;

public sealed class DefendLoseState : State
{
    private readonly DefendGameController _controller;

    public DefendLoseState(DefendGameController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
        _controller.Lose();
    }
}