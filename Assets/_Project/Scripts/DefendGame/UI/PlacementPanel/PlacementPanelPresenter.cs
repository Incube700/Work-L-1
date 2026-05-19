using System;
using Assets._Project.Scripts.UI.Common;

public sealed class PlacementPanelPresenter : IPresenter
{
    private readonly PlacementPanelView _view;
    private readonly PlacementSelectionService _selectionService;
    private readonly DefendPhaseService _phaseService;
    private readonly DefendLevelConfig _levelConfig;
    private readonly WalletService _walletService;

    private IReadOnlyReactiveVariable<int> _gold;
    private bool _isInitialized;

    public PlacementPanelPresenter(
        PlacementPanelView view,
        PlacementSelectionService selectionService,
        DefendPhaseService phaseService,
        DefendLevelConfig levelConfig,
        WalletService walletService)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _selectionService = selectionService ?? throw new ArgumentNullException(nameof(selectionService));
        _phaseService = phaseService ?? throw new ArgumentNullException(nameof(phaseService));
        _levelConfig = levelConfig ?? throw new ArgumentNullException(nameof(levelConfig));
        _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _view.MineSelected += OnMineSelected;
        _view.TurretSelected += OnTurretSelected;
        _view.PuddleSelected += OnPuddleSelected;

        _selectionService.SelectedTypeChanged += OnSelectedTypeChanged;
        _phaseService.PhaseChanged += OnPhaseChanged;

        _gold = _walletService.GetReactive(CurrencyType.Gold);
        _gold.Changed += OnGoldChanged;

        RefreshAll();

        _isInitialized = true;
    }

    public void Dispose()
    {
        if (_isInitialized == false)
        {
            return;
        }

        _phaseService.PhaseChanged -= OnPhaseChanged;
        _selectionService.SelectedTypeChanged -= OnSelectedTypeChanged;

        if (_gold != null)
        {
            _gold.Changed -= OnGoldChanged;
            _gold = null;
        }

        _view.PuddleSelected -= OnPuddleSelected;
        _view.TurretSelected -= OnTurretSelected;
        _view.MineSelected -= OnMineSelected;

        _isInitialized = false;
    }

    private void OnMineSelected()
    {
        _selectionService.Select(PlaceableType.Mine);
    }

    private void OnTurretSelected()
    {
        _selectionService.Select(PlaceableType.Turret);
    }

    private void OnPuddleSelected()
    {
        _selectionService.Select(PlaceableType.Puddle);
    }

    private void OnSelectedTypeChanged()
    {
        RefreshSelection();
    }

    private void OnGoldChanged()
    {
        RefreshAffordability();
    }

    private void OnPhaseChanged()
    {
        RefreshVisible();
    }

    private void RefreshAll()
    {
        RefreshVisible();
        RefreshSelection();
        RefreshCosts();
        RefreshAffordability();
    }

    private void RefreshVisible()
    {
        _view.SetVisible(_phaseService.IsRest);
    }

    private void RefreshSelection()
    {
        _view.SetSelected(_selectionService.SelectedType);
    }

    private void RefreshCosts()
    {
        _view.SetCosts(
            _levelConfig.MineConfig.CostGold,
            _levelConfig.TurretConfig.CostGold,
            _levelConfig.PuddleConfig.CostGold);
    }

    private void RefreshAffordability()
    {
        int gold = _walletService.Get(CurrencyType.Gold);

        _view.SetAffordable(
            gold >= _levelConfig.MineConfig.CostGold,
            gold >= _levelConfig.TurretConfig.CostGold,
            gold >= _levelConfig.PuddleConfig.CostGold);
    }
}
