using Assets._Project.Scripts.Utilities.StateMachineCore;
using Assets._Project.Scripts.Utilities.Timers;

public sealed class DefendRestState : State, IUpdatableState
{
    private readonly DefendGameController _controller;
    private CooldownTimer _timer;

    public DefendRestState(DefendGameController controller)
    {
        _controller = controller;
    }

    public bool IsFinished => _timer != null && _timer.IsFinished;

    public override void Enter()
    {
        base.Enter();

        _controller.SetPhase(DefendPhase.Rest);

        _timer = new CooldownTimer(_controller.RestDurationSeconds);
        _timer.Reset();

        _controller.LogState($"[Defend] Rest started for {_controller.RestDurationSeconds:0.##}s");
    }

    public void Update(float deltaTime)
    {
        _timer?.Tick(deltaTime);
    }
}