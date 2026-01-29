public sealed class WalletService
{
    public IReadOnlyReactiveVariable<int> Gold => _gold;
    public int GoldValue => _gold.Value;

    private readonly ReactiveVariable<int> _gold = new ReactiveVariable<int>(0);

    public void SetGold(int gold)
    {
        if (gold < 0) throw new System.ArgumentOutOfRangeException(nameof(gold));
        _gold.Value = gold;
    }

    public void AddGold(int value)
    {
        if (value <= 0) throw new System.ArgumentOutOfRangeException(nameof(value));
        _gold.Value += value;
    }

    public void SubtractGoldClamped(int value)
    {
        if (value <= 0) throw new System.ArgumentOutOfRangeException(nameof(value));

        int result = _gold.Value - value;
        _gold.Value = result < 0 ? 0 : result;
    }

    public bool TrySpendGold(int value)
    {
        if (value <= 0) throw new System.ArgumentOutOfRangeException(nameof(value));

        if (_gold.Value < value)
        {
            return false;
        }

        _gold.Value -= value;
        return true;
    }
}