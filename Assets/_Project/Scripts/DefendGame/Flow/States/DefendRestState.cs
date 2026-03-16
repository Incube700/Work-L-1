using Assets._Project.Scripts.Utilities.StateMachineCore;
using Assets._Project.Scripts.Utilities.Timers;
using UnityEngine;

public sealed class DefendRestState : State, IUpdatableState
{
    private readonly DefendPhaseService _phaseService;
    private readonly float _restDurationSeconds;

    private CooldownTimer _timer;

    public DefendRestState(
        DefendPhaseService phaseService,
        float restDurationSeconds)
    {
        _phaseService = phaseService;
        _restDurationSeconds = restDurationSeconds;
    }

    public bool IsFinished => _timer != null && _timer.IsFinished;

    public override void Enter()
    {
        base.Enter();

        _phaseService.SetPhase(DefendPhase.Rest);

        _timer = new CooldownTimer(_restDurationSeconds);
        _timer.Reset();

        Log($"[Defend] Rest started for {_restDurationSeconds:0.##}s");
    }

    public void Update(float deltaTime)
    {
        _timer?.Tick(deltaTime);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        Debug.Log(message);
    }
}