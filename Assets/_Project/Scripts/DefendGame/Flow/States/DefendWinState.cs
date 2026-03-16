using Assets._Project.Scripts.Utilities.StateMachineCore;

public sealed class DefendWinState : State
{
    private readonly DefendResultService _resultService;

    public DefendWinState(DefendResultService resultService)
    {
        _resultService = resultService;
    }

    public override void Enter()
    {
        base.Enter();
        _resultService.Win();
    }
}