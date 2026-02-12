public sealed class PlayerProgressService
{
    private readonly GameStatsService _stats;
    private readonly WalletService _wallet;
    private readonly ConfigService _configs;
    private readonly SaveService _save;

    private EconomyConfig _economy;

    public int Wins => _stats.WinsValue;
    public int Losses => _stats.LossesValue;

    public PlayerProgressService(GameStatsService stats, WalletService wallet, ConfigService configs, SaveService save)
    {
        _stats = stats;
        _wallet = wallet;
        _configs = configs;
        _save = save;

        _economy = _configs.Load<EconomyConfig>();
    }

    public void RegisterWin()
    {
        _stats.AddWin();
        _wallet.Add(CurrencyType.Gold, _economy.WinGold);
        _save.SaveAll();
    }

    public void RegisterLoss()
    {
        _stats.AddLoss();
        _wallet.SubtractClamped(CurrencyType.Gold, _economy.LoseGold);
        _save.SaveAll();
    }
}