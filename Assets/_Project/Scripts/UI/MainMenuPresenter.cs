using Assets._Project.Scripts.UI.Common;

public sealed class MainMenuPresenter : IPresenter
{
    private readonly MainMenuView _view;
    private readonly GameFlowService _flow;
    private readonly ConfigService _configs;
    private readonly ProgressResetService _reset;
    private readonly PopupService _popups;
    private readonly CurrencyListPresenter _currencyList;
    private readonly StatsPresenter _statsPresenter;
    private readonly PermanentUpgradesMenuPresenter _permanentUpgradesPresenter;

    private bool _isInitialized;

    public MainMenuPresenter(
        MainMenuView view,
        GameFlowService flow,
        ConfigService configs,
        ProgressResetService reset,
        PopupService popups,
        CurrencyListPresenter currencyList,
        StatsPresenter statsPresenter,
        PermanentUpgradesMenuPresenter permanentUpgradesPresenter)
    {
        _view = view;
        _flow = flow;
        _configs = configs;
        _reset = reset;
        _popups = popups;
        _currencyList = currencyList;
        _statsPresenter = statsPresenter;
        _permanentUpgradesPresenter = permanentUpgradesPresenter;
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        EconomyConfig economy = _configs.Load<EconomyConfig>();
        _view.SetResetCost(economy.ResetCost);

        _view.PlayClicked += OnPlayClicked;
        _view.ResetClicked += OnResetClicked;

        _currencyList.Initialize();
        _statsPresenter.Initialize();
        _permanentUpgradesPresenter.Initialize();

        _view.SetStatus(string.Empty);

        _isInitialized = true;
    }

    public void Dispose()
    {
        if (_isInitialized == false)
        {
            return;
        }

        _permanentUpgradesPresenter.Dispose();
        _statsPresenter.Dispose();
        _currencyList.Dispose();

        _view.PlayClicked -= OnPlayClicked;
        _view.ResetClicked -= OnResetClicked;

        _isInitialized = false;
    }

    private void OnPlayClicked()
    {
        DefendLevelsConfig levels = _configs.Load<DefendLevelsConfig>();
        DefendLevelConfig level = levels.GetRandom();

        UnityEngine.Debug.Log($"[Defend] Selected level: {level.name}");
        _flow.OpenDefendGameplay(level);
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
            _popups.OpenMessagePopup("Not enough currency", reason);
        }
    }
}
