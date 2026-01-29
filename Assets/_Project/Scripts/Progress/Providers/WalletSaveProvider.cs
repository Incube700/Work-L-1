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
            _wallet.SetGold(data.gold);
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
            gold = _wallet.GoldValue
        };

        _repo.Save(data);
    }

    public void Delete()
    {
        _repo.Delete<WalletData>();
    }
}