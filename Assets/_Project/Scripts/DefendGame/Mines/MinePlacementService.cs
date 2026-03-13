using UnityEngine;

public sealed class MinePlacementService
{
    private readonly DefendLevelConfig _level;
    private readonly WalletService _wallet;
    private readonly MineFactory _mineFactory;

    public MinePlacementService(
        DefendLevelConfig level,
        WalletService wallet,
        MineFactory mineFactory)
    {
        _level = level;
        _wallet = wallet;
        _mineFactory = mineFactory;
    }

    public bool TryPlace(Vector3 point)
    {
        if (_wallet.TrySpend(CurrencyType.Gold, _level.MineCostGold) == false)
        {
            Log(
                $"[Defend] Mine not placed. Need {_level.MineCostGold} gold, current {_wallet.Get(CurrencyType.Gold)}");
            return false;
        }

        _mineFactory.Create(point);
        Log($"[Defend] Mine placed at {point}");
        return true;
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        Debug.Log(message);
    }
}