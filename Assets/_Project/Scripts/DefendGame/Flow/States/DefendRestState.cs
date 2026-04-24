using Assets._Project.Scripts.Utilities.StateMachineCore;
using Assets._Project.Scripts.Utilities.Timers;
using UnityEngine;

public sealed class DefendRestState : State, IUpdatableState
{
    private readonly DefendPhaseService _phaseService;
    private readonly RestTimerService _restTimerService;
    private readonly float _restDurationSeconds;

    private CooldownTimer _timer;

    public DefendRestState(
        DefendPhaseService phaseService,
        RestTimerService restTimerService,
        float restDurationSeconds)
    {
        _phaseService = phaseService;
        _restTimerService = restTimerService;
        _restDurationSeconds = restDurationSeconds;
    }

    public bool IsFinished => _timer != null && _timer.IsFinished;

    public override void Enter()
    {
        base.Enter();

        _phaseService.SetPhase(DefendPhase.Rest);

        _timer = new CooldownTimer(_restDurationSeconds);
        _timer.Reset();

        _restTimerService.Start(_restDurationSeconds);

        Log($"[Defend] Rest started for {_restDurationSeconds:0.##}s");
    }

    public override void Exit()
    {
        base.Exit();

        _restTimerService.Stop();
    }

    public void Update(float deltaTime)
    {
        _timer?.Tick(deltaTime);
        _restTimerService.Tick(deltaTime);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        Debug.Log(message);
    }
}