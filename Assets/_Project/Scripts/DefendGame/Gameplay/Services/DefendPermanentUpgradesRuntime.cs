using System;
using System.Collections.Generic;

public sealed class DefendPermanentUpgradesRuntime : IDisposable
{
    private readonly List<IDefendPermanentUpgradeEffect> _effects =
        new List<IDefendPermanentUpgradeEffect>();

    private readonly PlayerExplosionDamageUpgradeEffect _playerExplosionDamageUpgradeEffect;

    private bool _isInitialized;

    public DefendPermanentUpgradesRuntime(
        PermanentUpgradesService permanentUpgradesService,
        ConfigService configService,
        BuildingStateService buildingStateService,
        WaveProgressService waveProgressService,
        EnemyService enemyService)
    {
        if (permanentUpgradesService == null)
        {
            throw new ArgumentNullException(nameof(permanentUpgradesService));
        }

        if (configService == null)
        {
            throw new ArgumentNullException(nameof(configService));
        }

        if (buildingStateService == null)
        {
            throw new ArgumentNullException(nameof(buildingStateService));
        }

        if (waveProgressService == null)
        {
            throw new ArgumentNullException(nameof(waveProgressService));
        }

        if (enemyService == null)
        {
            throw new ArgumentNullException(nameof(enemyService));
        }

        PermanentUpgradesConfig config = configService.Load<PermanentUpgradesConfig>();

        _playerExplosionDamageUpgradeEffect = new PlayerExplosionDamageUpgradeEffect(
            permanentUpgradesService,
            config);

        _effects.Add(new WaveHealUpgradeEffect(
            permanentUpgradesService,
            config,
            buildingStateService,
            waveProgressService));

        _effects.Add(new OpeningStrikeUpgradeEffect(
            permanentUpgradesService,
            config,
            waveProgressService,
            enemyService));

        _effects.Add(_playerExplosionDamageUpgradeEffect);
    }

    public float PlayerExplosionDamageMultiplier => _playerExplosionDamageUpgradeEffect.DamageMultiplier;

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        for (int i = 0; i < _effects.Count; i++)
        {
            _effects[i].Initialize();
        }

        _isInitialized = true;
    }

    public void Dispose()
    {
        if (_isInitialized == false)
        {
            return;
        }

        for (int i = _effects.Count - 1; i >= 0; i--)
        {
            _effects[i].Dispose();
        }

        _isInitialized = false;
    }
}