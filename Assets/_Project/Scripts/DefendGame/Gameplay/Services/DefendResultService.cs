using System;

public sealed class DefendResultService
{
    private readonly DefendLevelConfig _level;
    private readonly PlayerProgressService _progress;
    private readonly DefendPhaseService _phaseService;

    public event Action<DefendGameResult> ResultChanged;

    public DefendResultService(
        DefendLevelConfig level,
        PlayerProgressService progress,
        DefendPhaseService phaseService)
    {
        _level = level ?? throw new ArgumentNullException(nameof(level));
        _progress = progress ?? throw new ArgumentNullException(nameof(progress));
        _phaseService = phaseService ?? throw new ArgumentNullException(nameof(phaseService));
    }

    public DefendGameResult Result { get; private set; } = DefendGameResult.None;

    public bool HasResult => Result != DefendGameResult.None;

    public void Win()
    {
        if (HasResult)
        {
            return;
        }

        _phaseService.SetPhase(DefendPhase.Ended);
        _progress.RegisterWin(_level.WinRewardGold, _level.WinRewardDiamonds);

        Result = DefendGameResult.Win;
        ResultChanged?.Invoke(Result);
    }

    public void Lose()
    {
        if (HasResult)
        {
            return;
        }

        _phaseService.SetPhase(DefendPhase.Ended);
        _progress.RegisterLoss();

        Result = DefendGameResult.Lose;
        ResultChanged?.Invoke(Result);
    }
}