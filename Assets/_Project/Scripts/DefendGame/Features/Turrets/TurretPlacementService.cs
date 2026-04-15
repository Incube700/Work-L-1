using UnityEngine;

public sealed class TurretPlacementService
{
    private readonly DefendLevelConfig _level;
    private readonly WalletService _wallet;
    private readonly TurretFactory _turretFactory;

    public TurretPlacementService(
        DefendLevelConfig level,
        WalletService wallet,
        TurretFactory turretFactory)
    {
        _level = level;
        _wallet = wallet;
        _turretFactory = turretFactory;
    }

    public bool TryPlace(Vector3 point)
    {
        if (_wallet.TrySpend(CurrencyType.Gold, _level.TurretConfig.CostGold) == false)
        {
            Log(
                $"[Defend] Turret not placed. Need {_level.TurretConfig.CostGold} gold, current {_wallet.Get(CurrencyType.Gold)}");
            return false;
        }

        _turretFactory.Create(point);
        Log($"[Defend] Turret placed at {point}");
        return true;
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        Debug.Log(message);
    }
}