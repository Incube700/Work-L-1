using Assets._Project.Scripts.Utilities.StateMachineCore;

public sealed class DefendLoseState : State
{
    private readonly DefendResultService _resultService;

    public DefendLoseState(DefendResultService resultService)
    {
        _resultService = resultService;
    }

    public override void Enter()
    {
        base.Enter();
        _resultService.Lose();
    }
}