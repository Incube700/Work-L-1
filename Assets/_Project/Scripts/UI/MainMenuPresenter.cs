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

    public MainMenuPresenter(
        MainMenuView view,
        GameFlowService flow,
        ConfigService configs,
        ProgressResetService reset,
        PopupService popups,
        CurrencyListPresenter currencyList,
        StatsPresenter statsPresenter)
    {
        _view = view;
        _flow = flow;
        _configs = configs;
        _reset = reset;
        _popups = popups;
        _currencyList = currencyList;
        _statsPresenter = statsPresenter;
    }

    public void Initialize()
    {
        EconomyConfig economy = _configs.Load<EconomyConfig>();
        _view.SetResetCost(economy.ResetCost);

        _view.NumbersClicked += OnNumbersClicked;
        _view.LettersClicked += OnLettersClicked;
        _view.ResetClicked += OnResetClicked;

        _currencyList.Initialize();
        _statsPresenter.Initialize();

        _view.SetStatus(string.Empty);
    }

    public void Dispose()
    {
        _statsPresenter.Dispose();
        _currencyList.Dispose();

        _view.NumbersClicked -= OnNumbersClicked;
        _view.LettersClicked -= OnLettersClicked;
        _view.ResetClicked -= OnResetClicked;
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
            _popups.OpenMessagePopup("Not enough currency", reason);
        }
    }
}