using UnityEngine;

public sealed class PuddlePlacementService
{
    private readonly DefendLevelConfig _level;
    private readonly WalletService _wallet;
    private readonly PuddleFactory _puddleFactory;

    public PuddlePlacementService(
        DefendLevelConfig level,
        WalletService wallet,
        PuddleFactory puddleFactory)
    {
        _level = level;
        _wallet = wallet;
        _puddleFactory = puddleFactory;
    }

    public bool TryPlace(Vector3 point)
    {
        if (_wallet.TrySpend(CurrencyType.Gold, _level.PuddleConfig.CostGold) == false)
        {
            Log(
                $"[Defend] Puddle not placed. Need {_level.PuddleConfig.CostGold} gold, current {_wallet.Get(CurrencyType.Gold)}");
            return false;
        }

        _puddleFactory.Create(point);
        Log($"[Defend] Puddle placed at {point}");
        return true;
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        Debug.Log(message);
    }
}