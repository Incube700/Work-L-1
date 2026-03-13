using Assets._Project.Scripts.Utilities.StateMachineCore;

public sealed class DefendWinState : State
{
    private readonly DefendGameController _controller;

    public DefendWinState(DefendGameController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
        _controller.Win();
    }
}