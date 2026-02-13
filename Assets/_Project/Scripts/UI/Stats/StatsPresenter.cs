public sealed class StatsPresenter
{
    private readonly StatsView _view;
    private readonly GameStatsService _stats;

    public StatsPresenter(StatsView view, GameStatsService stats)
    {
        _view = view;
        _stats = stats;
    }

    public void Initialize()
    {
        _stats.Wins.Changed += OnChanged;
        _stats.Losses.Changed += OnChanged;

        Refresh();
    }

    public void Dispose()
    {
        _stats.Wins.Changed -= OnChanged;
        _stats.Losses.Changed -= OnChanged;
    }

    private void OnChanged() => Refresh();

    private void Refresh()
    {
        _view.SetWins(_stats.WinsValue);
        _view.SetLosses(_stats.LossesValue);
    }
}