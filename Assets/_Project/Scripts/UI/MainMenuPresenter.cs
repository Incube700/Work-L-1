public sealed class MainMenuPresenter
{
    private readonly MainMenuView _view;
    private readonly SceneLoader _sceneLoader;
    private readonly ConfigService _configs;
    private readonly GameStatsService _stats;
    private readonly WalletService _wallet;
    private readonly ProgressResetService _reset;

    public MainMenuPresenter(
        MainMenuView view,
        SceneLoader sceneLoader,
        ConfigService configs,
        GameStatsService stats,
        WalletService wallet,
        ProgressResetService reset)
    {
        _view = view;
        _sceneLoader = sceneLoader;
        _configs = configs;
        _stats = stats;
        _wallet = wallet;
        _reset = reset;
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
        _wallet.Gold.Changed += OnGoldChanged;

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
        _wallet.Gold.Changed -= OnGoldChanged;
    }

    private void RefreshAll()
    {
        _view.SetWins(_stats.WinsValue);
        _view.SetLosses(_stats.LossesValue);
        _view.SetGold(_wallet.GoldValue);
    }

    private void OnStatsChanged()
    {
        _view.SetWins(_stats.WinsValue);
        _view.SetLosses(_stats.LossesValue);
    }

    private void OnGoldChanged()
    {
        _view.SetGold(_wallet.GoldValue);
    }

    private void OnNumbersClicked()
    {
        _sceneLoader.Load(SceneNames.Gameplay, new GameplayArgs(GameMode.Numbers));
    }

    private void OnLettersClicked()
    {
        _sceneLoader.Load(SceneNames.Gameplay, new GameplayArgs(GameMode.Letters));
    }

    private void OnResetClicked()
    {
        if (_reset.TryResetStats(out string reason))
        {
            _view.SetStatus("Progress reset.");
        }
        else
        {
            _view.SetStatus(reason);
        }
    }
}