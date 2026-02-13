public sealed class MainMenuPresenter
{
    private readonly MainMenuView _view;
    private readonly GameFlowService _flow;
    private readonly ConfigService _configs;
    private readonly GameStatsService _stats;
    private readonly ProgressResetService _reset;
    private readonly PopupService _popups;
    private readonly CurrencyListPresenter _currencyList;

    public MainMenuPresenter(
        MainMenuView view,
        GameFlowService flow,
        ConfigService configs,
        GameStatsService stats,
        ProgressResetService reset,
        PopupService popups,
        CurrencyListPresenter currencyList)
    {
        _view = view;
        _flow = flow;
        _configs = configs;
        _stats = stats;
        _reset = reset;
        _popups = popups;
        _currencyList = currencyList;
    }

    public void Start()
    {
        EconomyConfig economy = _configs.Load<EconomyConfig>();
        _view.SetResetCost(economy.ResetCost);

        _view.NumbersClicked += OnNumbersClicked;
        _view.LettersClicked += OnLettersClicked;
        _view.ResetClicked += OnResetClicked;

        _stats.Wins.Changed += OnStatsChanged;
        _stats.Losses.Changed += OnStatsChanged;

        _currencyList.Initialize();

        RefreshStats();
        _view.SetStatus(string.Empty);
    }

    public void Stop()
    {
        _view.NumbersClicked -= OnNumbersClicked;
        _view.LettersClicked -= OnLettersClicked;
        _view.ResetClicked -= OnResetClicked;

        _stats.Wins.Changed -= OnStatsChanged;
        _stats.Losses.Changed -= OnStatsChanged;

        _currencyList.Dispose();
    }

    private void RefreshStats()
    {
        _view.SetWins(_stats.WinsValue);
        _view.SetLosses(_stats.LossesValue);
    }

    private void OnStatsChanged()
    {
        RefreshStats();
    }

    private void OnNumbersClicked()
    {
        _flow.OpenGameplay(GameMode.Numbers);
    }

    private void OnLettersClicked()
    {
        _flow.OpenGameplay(GameMode.Letters);
    }

    private void OnResetClicked()
    {
        if (_reset.TryResetStats(out string reason))
        {
            _view.SetStatus("Progress reset.");
        }
        else
        {
            _view.SetStatus(string.Empty);
            _popups.OpenMessagePopup("Not enough gold", reason);
        }
    }
}