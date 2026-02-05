public sealed class WalletSaveProvider : ISaveProvider
{
    private readonly SaveRepository _repo;
    private readonly WalletService _wallet;
    private readonly ConfigService _configs;

    public WalletSaveProvider(SaveRepository repo, WalletService wallet, ConfigService configs)
    {
        _repo = repo;
        _wallet = wallet;
        _configs = configs;
    }

    public void Load()
    {
        if (_repo.TryLoad(out WalletData data))
        {
            if (data.currencies != null && data.currencies.Count > 0)
            {
                for (int i = 0; i < data.currencies.Count; i++)
                {
                    CurrencyAmountData item = data.currencies[i];

                    if (item.amount < 0)
                    {
                        throw new System.InvalidOperationException("Wallet save is corrupted. Negative amount.");
                    }

                    _wallet.Set(item.type, item.amount);
                }

                return;
            }

            // миграция со старого сейва
            _wallet.SetGold(data.gold);
            Save();
            return;
        }

        EconomyConfig economy = _configs.Load<EconomyConfig>();
        _wallet.SetGold(economy.StartGold);
        Save();
    }

    public void Save()
    {
        WalletData data = new WalletData
        {
            gold = _wallet.GoldValue,
            currencies = _wallet.CreateSnapshot()
        };

        _repo.Save(data);
    }

    public void Delete()
    {
        _repo.Delete<WalletData>();
    }
}