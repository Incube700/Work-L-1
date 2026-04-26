using System;

public sealed class PermanentUpgradeEffectFactory
{
    private readonly BuildingStateService _buildingStateService;
    private readonly WaveProgressService _waveProgressService;
    private readonly EnemyService _enemyService;

    public PermanentUpgradeEffectFactory(
        BuildingStateService buildingStateService,
        WaveProgressService waveProgressService,
        EnemyService enemyService)
    {
        _buildingStateService = buildingStateService ?? throw new ArgumentNullException(nameof(buildingStateService));
        _waveProgressService = waveProgressService ?? throw new ArgumentNullException(nameof(waveProgressService));
        _enemyService = enemyService ?? throw new ArgumentNullException(nameof(enemyService));
    }

    public IDefendPermanentUpgradeEffect Create(PermanentUpgradeConfigBase config)
    {
        if (config == null)
        {
            throw new ArgumentNullException(nameof(config));
        }

        switch (config)
        {
            case WaveHealUpgradeConfig waveHealConfig:
                return new WaveHealUpgradeEffect(
                    waveHealConfig,
                    _buildingStateService,
                    _waveProgressService);

            case OpeningStrikeUpgradeConfig openingStrikeConfig:
                return new OpeningStrikeUpgradeEffect(
                    openingStrikeConfig,
                    _waveProgressService,
                    _enemyService);

            case PlayerExplosionDamageUpgradeConfig playerExplosionDamageConfig:
                return new PlayerExplosionDamageUpgradeEffect(playerExplosionDamageConfig);

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(config),
                    config.GetType().Name,
                    "Unsupported permanent upgrade config type.");
        }
    }
}