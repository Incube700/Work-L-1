using System;
using Assets._Project.Scripts.UI.Common;

public sealed class PlacementPanelPresenter : IPresenter
{
    private readonly PlacementPanelView _view;
    private readonly PlacementSelectionService _selectionService;
    private readonly DefendPhaseService _phaseService;
    private readonly DefendLevelConfig _levelConfig;

    private bool _isInitialized;

    public PlacementPanelPresenter(
        PlacementPanelView view,
        PlacementSelectionService selectionService,
        DefendPhaseService phaseService,
        DefendLevelConfig levelConfig)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _selectionService = selectionService ?? throw new ArgumentNullException(nameof(selectionService));
        _phaseService = phaseService ?? throw new ArgumentNullException(nameof(phaseService));
        _levelConfig = levelConfig ?? throw new ArgumentNullException(nameof(levelConfig));
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

    private void OnPhaseChanged()
    {
        RefreshVisible();
    }

    private void RefreshAll()
    {
        RefreshVisible();
        RefreshSelection();
        RefreshCosts();
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
}