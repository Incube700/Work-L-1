public sealed class GameStatsService
{
    public IReadOnlyReactiveVariable<int> Wins => _wins;
    public IReadOnlyReactiveVariable<int> Losses => _losses;

    public int WinsValue => _wins.Value;
    public int LossesValue => _losses.Value;

    private readonly ReactiveVariable<int> _wins = new ReactiveVariable<int>(0);
    private readonly ReactiveVariable<int> _losses = new ReactiveVariable<int>(0);

    public void Set(int wins, int losses)
    {
        if (wins < 0) throw new System.ArgumentOutOfRangeException(nameof(wins));
        if (losses < 0) throw new System.ArgumentOutOfRangeException(nameof(losses));

        _wins.Value = wins;
        _losses.Value = losses;
    }

    public void AddWin() => _wins.Value++;
    public void AddLoss() => _losses.Value++;

    public void Reset()
    {
        _wins.Value = 0;
        _losses.Value = 0;
    }
}