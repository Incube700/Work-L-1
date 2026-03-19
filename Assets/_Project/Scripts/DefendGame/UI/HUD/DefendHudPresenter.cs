using System;
using Assets._Project.Scripts.UI.Common;

public sealed class DefendHudPresenter : IPresenter
{
    private readonly DefendHudView _view;
    private readonly DefendPhaseService _phaseService;
    private readonly WaveProgressService _waveProgressService;
    private readonly BuildingStateService _buildingStateService;
    private readonly WalletService _walletService;

    private IReadOnlyReactiveVariable<int> _gold;

    public DefendHudPresenter(
        DefendHudView view,
        DefendPhaseService phaseService,
        WaveProgressService waveProgressService,
        BuildingStateService buildingStateService,
        WalletService walletService)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _phaseService = phaseService ?? throw new ArgumentNullException(nameof(phaseService));
        _waveProgressService = waveProgressService ?? throw new ArgumentNullException(nameof(waveProgressService));
        _buildingStateService = buildingStateService ?? throw new ArgumentNullException(nameof(buildingStateService));
        _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
    }

    public void Initialize()
    {
        _phaseService.PhaseChanged += OnPhaseChanged;
        _waveProgressService.CurrentWaveChanged += OnCurrentWaveChanged;
        _buildingStateService.BuildingChanged += OnBuildingChanged;
        _buildingStateService.HealthChanged += OnHealthChanged;

        _gold = _walletService.GetReactive(CurrencyType.Gold);
        _gold.Changed += OnGoldChanged;

        RefreshAll();
    }

    public void Dispose()
    {
        _phaseService.PhaseChanged -= OnPhaseChanged;
        _waveProgressService.CurrentWaveChanged -= OnCurrentWaveChanged;
        _buildingStateService.BuildingChanged -= OnBuildingChanged;
        _buildingStateService.HealthChanged -= OnHealthChanged;

        if (_gold != null)
        {
            _gold.Changed -= OnGoldChanged;
            _gold = null;
        }
    }

    private void OnPhaseChanged()
    {
        RefreshPhase();
    }

    private void OnCurrentWaveChanged()
    {
        RefreshWave();
    }

    private void OnBuildingChanged()
    {
        RefreshBuildingHealth();
    }

    private void OnHealthChanged()
    {
        RefreshBuildingHealth();
    }

    private void OnGoldChanged()
    {
        RefreshGold();
    }

    private void RefreshAll()
    {
        RefreshGold();
        RefreshWave();
        RefreshPhase();
        RefreshBuildingHealth();
    }

    private void RefreshGold()
    {
        _view.SetGold(_walletService.Get(CurrencyType.Gold));
    }

    private void RefreshWave()
    {
        int currentWave = _waveProgressService.CurrentWaveNumber;

        if (currentWave <= 0 && _waveProgressService.HasAnyWaves)
        {
            currentWave = 1;
        }

        _view.SetWave(currentWave, _waveProgressService.WavesCount);
    }

    private void RefreshPhase()
    {
        _view.SetPhase(_phaseService.CurrentPhase.ToString());
    }

    private void RefreshBuildingHealth()
    {
        _view.SetBuildingHealth(
            _buildingStateService.CurrentHealth,
            _buildingStateService.MaxHealth);
    }
}