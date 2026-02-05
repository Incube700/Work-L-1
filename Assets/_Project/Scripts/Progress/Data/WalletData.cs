using System;
using System.Collections.Generic;

[Serializable]
public sealed class WalletData
{
    // старое поле оставляем, чтобы старые сейвы не сломались
    public int gold;

    // новая схема (JsonUtility умеет List)
    public List<CurrencyAmountData> currencies;
}

[Serializable]
public sealed class CurrencyAmountData
{
    public CurrencyType type;
    public int amount;
}