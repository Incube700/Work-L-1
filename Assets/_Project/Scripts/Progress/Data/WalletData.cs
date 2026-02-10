using System;
using System.Collections.Generic;

[Serializable]
public sealed class WalletData
{
    //  старые сейвы 
    public int gold;

    // новые сейвы
    public List<CurrencyAmountData> currencies;
}

[Serializable]
public sealed class CurrencyAmountData
{
    public CurrencyType type;
    public int amount;
}