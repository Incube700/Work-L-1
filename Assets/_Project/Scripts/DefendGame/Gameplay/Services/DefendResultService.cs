using System;

public sealed class DefendResultService
{
    private readonly DefendLevelConfig _level;
    private readonly PlayerProgressService _progress;
    private readonly GameFlowService _flow;
    private readonly DefendPhaseService _phaseService;

    public DefendResultService(
        DefendLevelConfig level,
        PlayerProgressService progress,
        GameFlowService flow,
        DefendPhaseService phaseService)
    {
        _level = level ?? throw new ArgumentNullException(nameof(level));
        _progress = progress ?? throw new ArgumentNullException(nameof(progress));
        _flow = flow ?? throw new ArgumentNullException(nameof(flow));
        _phaseService = phaseService ?? throw new ArgumentNullException(nameof(phaseService));
    }

    public void Win()
    {
        _phaseService.SetPhase(DefendPhase.Ended);
        _progress.RegisterWin(_level.WinRewardGold);
        _flow.OpenMainMenu();
    }

    public void Lose()
    {
        _phaseService.SetPhase(DefendPhase.Ended);
        _progress.RegisterLoss();
        _flow.OpenMainMenu();
    }
}