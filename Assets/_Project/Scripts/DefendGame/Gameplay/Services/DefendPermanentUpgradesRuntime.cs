using System;
using System.Collections.Generic;

public sealed class DefendPermanentUpgradesRuntime : IDisposable
{
    private readonly PermanentUpgradesService _permanentUpgradesService;
    private readonly PermanentUpgradeEffectFactory _effectFactory;
    private readonly List<IDefendPermanentUpgradeEffect> _effects =
        new List<IDefendPermanentUpgradeEffect>();

    private PlayerExplosionDamageUpgradeEffect _playerExplosionDamageUpgradeEffect;
    private bool _isInitialized;

    public DefendPermanentUpgradesRuntime(
        PermanentUpgradesService permanentUpgradesService,
        PermanentUpgradeEffectFactory effectFactory)
    {
        _permanentUpgradesService = permanentUpgradesService ??
            throw new ArgumentNullException(nameof(permanentUpgradesService));
        _effectFactory = effectFactory ?? throw new ArgumentNullException(nameof(effectFactory));
    }

    public float PlayerExplosionDamageMultiplier
    {
        get
        {
            if (_playerExplosionDamageUpgradeEffect == null)
            {
                return 1f;
            }

            return _playerExplosionDamageUpgradeEffect.DamageMultiplier;
        }
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        CreatePurchasedEffects();

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

        _effects.Clear();
        _playerExplosionDamageUpgradeEffect = null;
        _isInitialized = false;
    }

    private void CreatePurchasedEffects()
    {
        IReadOnlyList<PermanentUpgradeConfigBase> upgrades = _permanentUpgradesService.Upgrades;

        for (int i = 0; i < upgrades.Count; i++)
        {
            PermanentUpgradeConfigBase config = upgrades[i];

            if (config == null)
            {
                continue;
            }

            if (_permanentUpgradesService.IsPurchased(config.Type) == false)
            {
                continue;
            }

            IDefendPermanentUpgradeEffect effect = _effectFactory.Create(config);
            _effects.Add(effect);

            if (effect is PlayerExplosionDamageUpgradeEffect playerExplosionDamageUpgradeEffect)
            {
                _playerExplosionDamageUpgradeEffect = playerExplosionDamageUpgradeEffect;
            }
        }
    }
}