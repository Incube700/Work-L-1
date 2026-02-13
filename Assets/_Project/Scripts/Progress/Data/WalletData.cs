using System;
using System.Collections.Generic;

[Serializable]
public sealed class WalletData
{
    public List<CurrencyAmountData> currencies;
}

[Serializable]
public sealed class CurrencyAmountData
{
    public CurrencyType type;
    public int amount;
}