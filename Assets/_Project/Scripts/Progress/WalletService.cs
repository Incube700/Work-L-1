using System;
using System.Collections.Generic;

public sealed class WalletService
{
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
    
    public CurrencyType[] GetAvailableCurrencies()
    {
        CurrencyType[] result = new CurrencyType[_currencies.Count];

        int index = 0;
        foreach (var pair in _currencies)
        {
            result[index] = pair.Key;
            index++;
        }

        return result;
    }
}
