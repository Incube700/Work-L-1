public sealed class MainMenuPresenter
{
    private readonly MainMenuView _view;
    private readonly GameFlowService _flow;
    private readonly ConfigService _configs;
    private readonly GameStatsService _stats;
    private readonly WalletService _wallet;
    private readonly ProgressResetService _reset;
    private readonly PopupService _popups;

    public MainMenuPresenter(
        MainMenuView view,
        GameFlowService flow,
        ConfigService configs,
        GameStatsService stats,
        WalletService wallet,
        ProgressResetService reset,
        PopupService popups)
    {
        _view = view;
        _flow = flow;
        _configs = configs;
        _stats = stats;
        _wallet = wallet;
        _reset = reset;
        _popups = popups;
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

        _wallet.GetReactive(CurrencyType.Gold).Changed += OnGoldChanged;

        RefreshAll();
        _view.SetStatus(string.Empty);
    }

    public void Stop()
    {
        _view.NumbersClicked -= OnNumbersClicked;
        _view.LettersClicked -= OnLettersClicked;
        _view.ResetClicked -= OnResetClicked;

        _stats.Wins.Changed -= OnStatsChanged;
        _stats.Losses.Changed -= OnStatsChanged;

        _wallet.GetReactive(CurrencyType.Gold).Changed -= OnGoldChanged;
    }

    private void RefreshAll()
    {
        _view.SetWins(_stats.WinsValue);
        _view.SetLosses(_stats.LossesValue);
        _view.SetGold(_wallet.Get(CurrencyType.Gold));
    }

    private void OnStatsChanged()
    {
        _view.SetWins(_stats.WinsValue);
        _view.SetLosses(_stats.LossesValue);
    }

    private void OnGoldChanged()
    {
        _view.SetGold(_wallet.Get(CurrencyType.Gold));
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