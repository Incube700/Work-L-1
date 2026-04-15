using System;

public sealed class PermanentUpgradesService
{
    private readonly WalletService _wallet;
    private readonly EconomyConfig _economy;

    private PermanentUpgradesData _data = new PermanentUpgradesData();

    public event Action Changed;

    public PermanentUpgradesService(WalletService wallet, ConfigService configs)
    {
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));

        if (configs == null)
        {
            throw new ArgumentNullException(nameof(configs));
        }

        _economy = configs.Load<EconomyConfig>();
    }

    public bool IsPurchased(PermanentUpgradeType type)
    {
        switch (type)
        {
            case PermanentUpgradeType.WaveHeal:
                return _data.waveHealPurchased;

            case PermanentUpgradeType.OpeningStrike:
                return _data.openingStrikePurchased;

            case PermanentUpgradeType.PlayerExplosionDamage:
                return _data.playerExplosionDamagePurchased;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public int GetCost(PermanentUpgradeType type)
    {
        switch (type)
        {
            case PermanentUpgradeType.WaveHeal:
                return _economy.WaveHealCostDiamonds;

            case PermanentUpgradeType.OpeningStrike:
                return _economy.OpeningStrikeCostDiamonds;

            case PermanentUpgradeType.PlayerExplosionDamage:
                return _economy.PlayerExplosionDamageCostDiamonds;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void Load(PermanentUpgradesData data)
    {
        _data = data ?? new PermanentUpgradesData();
        Changed?.Invoke();
    }

    public PermanentUpgradesData CreateSnapshot()
    {
        return new PermanentUpgradesData
        {
            waveHealPurchased = _data.waveHealPurchased,
            openingStrikePurchased = _data.openingStrikePurchased,
            playerExplosionDamagePurchased = _data.playerExplosionDamagePurchased,
        };
    }

    public bool TryPurchase(PermanentUpgradeType type, out string failureReason)
    {
        if (IsPurchased(type))
        {
            failureReason = "Ability already purchased.";
            return false;
        }

        int cost = GetCost(type);

        if (_wallet.TrySpend(CurrencyType.Diamond, cost) == false)
        {
            failureReason =
                $"Not enough diamonds. Need {cost}, have {_wallet.Get(CurrencyType.Diamond)}.";
            return false;
        }

        SetPurchased(type, true);
        Changed?.Invoke();

        failureReason = null;
        return true;
    }

    private void SetPurchased(PermanentUpgradeType type, bool isPurchased)
    {
        switch (type)
        {
            case PermanentUpgradeType.WaveHeal:
                _data.waveHealPurchased = isPurchased;
                break;

            case PermanentUpgradeType.OpeningStrike:
                _data.openingStrikePurchased = isPurchased;
                break;

            case PermanentUpgradeType.PlayerExplosionDamage:
                _data.playerExplosionDamagePurchased = isPurchased;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    public void Reset()
    {
        _data.waveHealPurchased = false;
        _data.openingStrikePurchased = false;
        _data.playerExplosionDamagePurchased = false;

        Changed?.Invoke();

        Log("[Meta] Permanent upgrades reset.");
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        UnityEngine.Debug.Log(message);
    }
}
