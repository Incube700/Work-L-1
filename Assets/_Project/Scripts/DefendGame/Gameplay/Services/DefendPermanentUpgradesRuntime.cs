using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;

public sealed class DefendPermanentUpgradesRuntime : IDisposable
{
    private readonly PermanentUpgradesService _permanentUpgradesService;
    private readonly EconomyConfig _economy;
    private readonly BuildingStateService _buildingStateService;
    private readonly WaveProgressService _waveProgressService;
    private readonly EnemyService _enemyService;

    private int _openingStrikeTargetsLeft;
    private bool _isInitialized;

    public DefendPermanentUpgradesRuntime(
        PermanentUpgradesService permanentUpgradesService,
        ConfigService configService,
        BuildingStateService buildingStateService,
        WaveProgressService waveProgressService,
        EnemyService enemyService)
    {
        _permanentUpgradesService = permanentUpgradesService ??
            throw new ArgumentNullException(nameof(permanentUpgradesService));
        _buildingStateService = buildingStateService ??
            throw new ArgumentNullException(nameof(buildingStateService));
        _waveProgressService = waveProgressService ??
            throw new ArgumentNullException(nameof(waveProgressService));
        _enemyService = enemyService ??
            throw new ArgumentNullException(nameof(enemyService));

        if (configService == null)
        {
            throw new ArgumentNullException(nameof(configService));
        }

        _economy = configService.Load<EconomyConfig>();
    }

    public float PlayerExplosionDamageMultiplier { get; private set; } = 1f;

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        PlayerExplosionDamageMultiplier = _permanentUpgradesService.IsPurchased(PermanentUpgradeType.PlayerExplosionDamage)
            ? 1f + _economy.PlayerExplosionDamagePercent / 100f
            : 1f;

        _waveProgressService.WaveStarted += OnWaveStarted;
        _enemyService.EnemyAdded += OnEnemyAdded;

        _openingStrikeTargetsLeft = 0;
        Log(
            $"[Meta] Upgrades initialized. " +
            $"WaveHeal={_permanentUpgradesService.IsPurchased(PermanentUpgradeType.WaveHeal)}, " +
            $"OpeningStrike={_permanentUpgradesService.IsPurchased(PermanentUpgradeType.OpeningStrike)}, " +
            $"PlayerExplosionDamage={_permanentUpgradesService.IsPurchased(PermanentUpgradeType.PlayerExplosionDamage)}, " +
            $"ExplosionMultiplier={PlayerExplosionDamageMultiplier}");
        _isInitialized = true;
    }

    public void Dispose()
    {
        if (_isInitialized == false)
        {
            return;
        }

        _enemyService.EnemyAdded -= OnEnemyAdded;
        _waveProgressService.WaveStarted -= OnWaveStarted;

        _openingStrikeTargetsLeft = 0;
        PlayerExplosionDamageMultiplier = 1f;
        _isInitialized = false;
    }

    private void OnWaveStarted(int waveIndex, WaveConfig wave)
    {
        Log($"[Meta] Wave started: {waveIndex}");

        if (_permanentUpgradesService.IsPurchased(PermanentUpgradeType.WaveHeal))
        {
            _buildingStateService.RestorePercent(_economy.WaveHealPercent);
            Log($"[Meta] WaveHeal applied: +{_economy.WaveHealPercent}% building HP.");
        }

        _openingStrikeTargetsLeft = _permanentUpgradesService.IsPurchased(PermanentUpgradeType.OpeningStrike)
            ? _economy.OpeningStrikeTargetsCount
            : 0;

        if (_openingStrikeTargetsLeft > 0)
        {
            Log($"[Meta] OpeningStrike ready. Targets: {_openingStrikeTargetsLeft}, damage: {_economy.OpeningStrikeDamagePercent}% max HP.");
        }
    }

    private void OnEnemyAdded(Entity enemy)
    {
        if (_openingStrikeTargetsLeft <= 0 || enemy == null)
        {
            return;
        }

        if (enemy.TryGetComponent(out MaxHealth maxHealth) == false)
        {
            return;
        }

        if (enemy.HasComponent<TakeDamageRequest>() == false)
        {
            return;
        }

        float damage = maxHealth.Value.Value * _economy.OpeningStrikeDamagePercent / 100f;

        _openingStrikeTargetsLeft--;

        if (damage > 0f)
        {
            enemy.TakeDamageRequest.Invoke(damage);
        }
        
        Log($"[Meta] OpeningStrike hit enemy. Damage={damage}, targets left={_openingStrikeTargetsLeft}");
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        UnityEngine.Debug.Log(message);
    }
}
