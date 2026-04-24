using System;

public sealed class PlayerExplosionDamageUpgradeEffect : IDefendPermanentUpgradeEffect
{
    private readonly PermanentUpgradesService _permanentUpgradesService;
    private readonly PermanentUpgradesConfig _config;

    public PlayerExplosionDamageUpgradeEffect(
        PermanentUpgradesService permanentUpgradesService,
        PermanentUpgradesConfig config)
    {
        _permanentUpgradesService = permanentUpgradesService ?? throw new ArgumentNullException(nameof(permanentUpgradesService));
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public float DamageMultiplier { get; private set; } = 1f;

    public void Initialize()
    {
        DamageMultiplier = _permanentUpgradesService.IsPurchased(PermanentUpgradeType.PlayerExplosionDamage)
            ? 1f + _config.PlayerExplosionDamagePercent / 100f
            : 1f;
    }

    public void Dispose()
    {
        DamageMultiplier = 1f;
    }
}