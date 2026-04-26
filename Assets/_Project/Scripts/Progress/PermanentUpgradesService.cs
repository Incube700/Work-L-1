using System;
using System.Collections.Generic;

public sealed class PermanentUpgradesService
{
    private readonly WalletService _wallet;
    private readonly PermanentUpgradesConfig _config;
    private readonly HashSet<PermanentUpgradeType> _purchasedTypes = new HashSet<PermanentUpgradeType>();

    public event Action Changed;

    public PermanentUpgradesService(WalletService wallet, ConfigService configs)
    {
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));

        if (configs == null)
        {
            throw new ArgumentNullException(nameof(configs));
        }

        _config = configs.Load<PermanentUpgradesConfig>();
    }

    public IReadOnlyList<PermanentUpgradeConfigBase> Upgrades => _config.Upgrades;

    public bool IsPurchased(PermanentUpgradeType type)
    {
        return _purchasedTypes.Contains(type);
    }

    public PermanentUpgradeConfigBase GetConfig(PermanentUpgradeType type)
    {
        return _config.GetUpgrade(type);
    }

    public int GetCost(PermanentUpgradeType type)
    {
        return GetConfig(type).CostDiamonds;
    }

    public void Load(PermanentUpgradesData data)
    {
        _purchasedTypes.Clear();

        if (data != null)
        {
            if (data.waveHealPurchased)
            {
                _purchasedTypes.Add(PermanentUpgradeType.WaveHeal);
            }

            if (data.openingStrikePurchased)
            {
                _purchasedTypes.Add(PermanentUpgradeType.OpeningStrike);
            }

            if (data.playerExplosionDamagePurchased)
            {
                _purchasedTypes.Add(PermanentUpgradeType.PlayerExplosionDamage);
            }
        }

        Changed?.Invoke();
    }

    public PermanentUpgradesData CreateSnapshot()
    {
        return new PermanentUpgradesData
        {
            waveHealPurchased = IsPurchased(PermanentUpgradeType.WaveHeal),
            openingStrikePurchased = IsPurchased(PermanentUpgradeType.OpeningStrike),
            playerExplosionDamagePurchased = IsPurchased(PermanentUpgradeType.PlayerExplosionDamage),
        };
    }

    public bool TryPurchase(PermanentUpgradeType type, out string failureReason)
    {
        if (IsPurchased(type))
        {
            failureReason = "Ability already purchased.";
            return false;
        }

        PermanentUpgradeConfigBase config = GetConfig(type);
        int cost = config.CostDiamonds;

        if (_wallet.TrySpend(CurrencyType.Diamond, cost) == false)
        {
            failureReason =
                $"Not enough diamonds. Need {cost}, have {_wallet.Get(CurrencyType.Diamond)}.";
            return false;
        }

        _purchasedTypes.Add(type);
        Changed?.Invoke();

        failureReason = null;
        return true;
    }

    public void Reset()
    {
        _purchasedTypes.Clear();
        Changed?.Invoke();

        Log("[Meta] Permanent upgrades reset.");
    }

    private static void Log(string message)
    {
        UnityEngine.Debug.Log(message);
    }
}