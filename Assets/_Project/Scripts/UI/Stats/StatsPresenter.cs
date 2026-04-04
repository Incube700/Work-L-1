public sealed class StatsPresenter
{
    private readonly StatsView _view;
    private readonly GameStatsService _stats;

    private bool _isInitialized;

    public StatsPresenter(StatsView view, GameStatsService stats)
    {
        _view = view;
        _stats = stats;
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _stats.Wins.Changed += OnChanged;
        _stats.Losses.Changed += OnChanged;

        Refresh();

        _isInitialized = true;
    }

    public void Dispose()
    {
        if (_isInitialized == false)
        {
            return;
        }

        _stats.Wins.Changed -= OnChanged;
        _stats.Losses.Changed -= OnChanged;

        _isInitialized = false;
    }

    private void OnChanged()
    {
        Refresh();
    }

    private void Refresh()
    {
        _view.SetWins(_stats.WinsValue);
        _view.SetLosses(_stats.LossesValue);
    }
}