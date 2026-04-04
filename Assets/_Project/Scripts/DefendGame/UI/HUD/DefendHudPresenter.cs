using System;
using Assets._Project.Scripts.UI.Common;

public sealed class DefendHudPresenter : IPresenter
{
    private readonly DefendHudView _view;
    private readonly DefendPhaseService _phaseService;
    private readonly WaveProgressService _waveProgressService;
    private readonly BuildingStateService _buildingStateService;

    private bool _isInitialized;

    public DefendHudPresenter(
        DefendHudView view,
        DefendPhaseService phaseService,
        WaveProgressService waveProgressService,
        BuildingStateService buildingStateService)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _phaseService = phaseService ?? throw new ArgumentNullException(nameof(phaseService));
        _waveProgressService = waveProgressService ?? throw new ArgumentNullException(nameof(waveProgressService));
        _buildingStateService = buildingStateService ?? throw new ArgumentNullException(nameof(buildingStateService));
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _phaseService.PhaseChanged += OnPhaseChanged;
        _waveProgressService.CurrentWaveChanged += OnCurrentWaveChanged;
        _buildingStateService.BuildingChanged += OnBuildingChanged;
        _buildingStateService.HealthChanged += OnHealthChanged;

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
        _waveProgressService.CurrentWaveChanged -= OnCurrentWaveChanged;
        _buildingStateService.BuildingChanged -= OnBuildingChanged;
        _buildingStateService.HealthChanged -= OnHealthChanged;

        _isInitialized = false;
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

    private void RefreshAll()
    {
        RefreshWave();
        RefreshPhase();
        RefreshBuildingHealth();
    }

    private void RefreshWave()
    {
        int currentWave = _waveProgressService.CurrentWaveNumber;

        if (currentWave <= 0 && _waveProgressService.HasAnyWaves == true)
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