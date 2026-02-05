using System;
using System.Collections.Generic;

public sealed class WalletService
{
    public IReadOnlyReactiveVariable<int> Gold => GetReactive(CurrencyType.Gold);
    public int GoldValue => Get(CurrencyType.Gold);

    private readonly Dictionary<CurrencyType, ReactiveVariable<int>> _currencies;

    public WalletService()
    {
        _currencies = new Dictionary<CurrencyType, ReactiveVariable<int>>();

        Array values = Enum.GetValues(typeof(CurrencyType));

        foreach (object value in values)
        {
            CurrencyType type = (CurrencyType)value;
            _currencies.Add(type, new ReactiveVariable<int>(0));
        }
    }

    // --- НОВЫЙ универсальный API ---

    public int Get(CurrencyType type)
    {
        return GetVariable(type).Value;
    }

    public IReadOnlyReactiveVariable<int> GetReactive(CurrencyType type)
    {
        return GetVariable(type);
    }

    public void Set(CurrencyType type, int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        GetVariable(type).Value = amount;
    }

    public void Add(CurrencyType type, int amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        ReactiveVariable<int> variable = GetVariable(type);
        variable.Value += amount;
    }

    public void SubtractClamped(CurrencyType type, int amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

        ReactiveVariable<int> variable = GetVariable(type);
        int result = variable.Value - amount;

        variable.Value = result < 0 ? 0 : result;
    }

    public bool TrySpend(CurrencyType type, int amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

        ReactiveVariable<int> variable = GetVariable(type);

        if (variable.Value < amount)
        {
            return false;
        }

        variable.Value -= amount;
        return true;
    }

    // --- СТАРЫЙ API оставляем для совместимости (твой проект не ломаем) ---

    public void SetGold(int gold) => Set(CurrencyType.Gold, gold);
    public void AddGold(int value) => Add(CurrencyType.Gold, value);
    public void SubtractGoldClamped(int value) => SubtractClamped(CurrencyType.Gold, value);
    public bool TrySpendGold(int value) => TrySpend(CurrencyType.Gold, value);

    public List<CurrencyAmountData> CreateSnapshot()
    {
        List<CurrencyAmountData> snapshot = new List<CurrencyAmountData>(_currencies.Count);

        foreach (KeyValuePair<CurrencyType, ReactiveVariable<int>> pair in _currencies)
        {
            snapshot.Add(new CurrencyAmountData
            {
                type = pair.Key,
                amount = pair.Value.Value
            });
        }

        return snapshot;
    }

    private ReactiveVariable<int> GetVariable(CurrencyType type)
    {
        if (_currencies.TryGetValue(type, out ReactiveVariable<int> variable) == false)
        {
            throw new InvalidOperationException($"Currency '{type}' is not registered.");
        }

        return variable;
    }
}
