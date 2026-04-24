using System;

public sealed class WaveHealUpgradeEffect : IDefendPermanentUpgradeEffect
{
    private readonly PermanentUpgradesService _permanentUpgradesService;
    private readonly PermanentUpgradesConfig _config;
    private readonly BuildingStateService _buildingStateService;
    private readonly WaveProgressService _waveProgressService;

    private bool _isInitialized;

    public WaveHealUpgradeEffect(
        PermanentUpgradesService permanentUpgradesService,
        PermanentUpgradesConfig config,
        BuildingStateService buildingStateService,
        WaveProgressService waveProgressService)
    {
        _permanentUpgradesService = permanentUpgradesService ?? throw new ArgumentNullException(nameof(permanentUpgradesService));
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

        if (_permanentUpgradesService.IsPurchased(PermanentUpgradeType.WaveHeal) == false)
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
        _buildingStateService.RestorePercent(_config.WaveHealPercent);
    }
}