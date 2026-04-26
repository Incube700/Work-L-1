using System;

public sealed class WaveHealUpgradeEffect : IDefendPermanentUpgradeEffect
{
    private readonly WaveHealUpgradeConfig _config;
    private readonly BuildingStateService _buildingStateService;
    private readonly WaveProgressService _waveProgressService;

    private bool _isInitialized;

    public WaveHealUpgradeEffect(
        WaveHealUpgradeConfig config,
        BuildingStateService buildingStateService,
        WaveProgressService waveProgressService)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _buildingStateService = buildingStateService ?? throw new ArgumentNullException(nameof(buildingStateService));
        _waveProgressService = waveProgressService ?? throw new ArgumentNullException(nameof(waveProgressService));
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _waveProgressService.WaveStarted += OnWaveStarted;
        _isInitialized = true;
    }

    public void Dispose()
    {
        if (_isInitialized == false)
        {
            return;
        }

        _waveProgressService.WaveStarted -= OnWaveStarted;
        _isInitialized = false;
    }

    private void OnWaveStarted(int waveIndex, WaveConfig wave)
    {
        _buildingStateService.RestorePercent(_config.HealPercent);
    }
}