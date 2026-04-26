using System;

public sealed class PlayerExplosionDamageUpgradeEffect : IDefendPermanentUpgradeEffect
{
    private readonly PlayerExplosionDamageUpgradeConfig _config;

    public PlayerExplosionDamageUpgradeEffect(PlayerExplosionDamageUpgradeConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public float DamageMultiplier { get; private set; } = 1f;

    public void Initialize()
    {
        DamageMultiplier = 1f + _config.DamagePercent / 100f;
    }

    public void Dispose()
    {
        DamageMultiplier = 1f;
    }
}