using System;

public sealed class WalletService
{
    public int Gold { get; private set; }

    private event Action GoldChanged;

    public void SetGold(int gold)
    {
        if (gold < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(gold));
        }
        
        Gold = gold;
        GoldChanged?.Invoke();
    }

    public void Add(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
        
        Gold += value;
        GoldChanged?.Invoke();
    }

    public void SubtractClamped(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
        
        Gold -= value;

        if (Gold < 0)
        {
            Gold = 0;
        }
        
        GoldChanged?.Invoke();
    }

    public bool TrySpend(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        if (Gold < value)
        {
            return false;
        }
        
        Gold -= value;
        GoldChanged?.Invoke();
        return true;
    }
}
    

