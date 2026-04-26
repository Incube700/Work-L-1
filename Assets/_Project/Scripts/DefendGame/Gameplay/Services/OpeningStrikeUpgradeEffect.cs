using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;

public sealed class OpeningStrikeUpgradeEffect : IDefendPermanentUpgradeEffect
{
    private readonly OpeningStrikeUpgradeConfig _config;
    private readonly WaveProgressService _waveProgressService;
    private readonly EnemyService _enemyService;

    private int _targetsLeft;
    private bool _isInitialized;

    public OpeningStrikeUpgradeEffect(
        OpeningStrikeUpgradeConfig config,
        WaveProgressService waveProgressService,
        EnemyService enemyService)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _waveProgressService = waveProgressService ?? throw new ArgumentNullException(nameof(waveProgressService));
        _enemyService = enemyService ?? throw new ArgumentNullException(nameof(enemyService));
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _waveProgressService.WaveStarted += OnWaveStarted;
        _enemyService.EnemyAdded += OnEnemyAdded;
        _targetsLeft = 0;
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

        _targetsLeft = 0;
        _isInitialized = false;
    }

    private void OnWaveStarted(int waveIndex, WaveConfig wave)
    {
        _targetsLeft = _config.TargetsCount;
    }

    private void OnEnemyAdded(Entity enemy)
    {
        if (_targetsLeft <= 0 || enemy == null)
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

        float damage = maxHealth.Value.Value * _config.DamagePercent / 100f;

        _targetsLeft--;

        if (damage > 0f)
        {
            enemy.TakeDamageRequest.Invoke(damage);
        }
    }
}