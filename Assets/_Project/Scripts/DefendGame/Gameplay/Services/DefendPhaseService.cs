using System;

public sealed class DefendPhaseService
{
    private DefendPhase _currentPhase = DefendPhase.Rest;

    public event Action PhaseChanged;

    public DefendPhase CurrentPhase => _currentPhase;

    public bool IsWave => _currentPhase == DefendPhase.Wave;
    public bool IsRest => _currentPhase == DefendPhase.Rest;
    public bool IsEnded => _currentPhase == DefendPhase.Ended;

    public void SetPhase(DefendPhase phase)
    {
        if (_currentPhase == phase)
        {
            return;
        }

        _currentPhase = phase;
        PhaseChanged?.Invoke();
    }
}