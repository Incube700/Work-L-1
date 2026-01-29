public sealed class PlayerProgressService
{
    private readonly GameStatsService _stats;
    private readonly WalletService _wallet;
    private readonly ConfigService _configs;
    private readonly SaveService _save;

    private EconomyConfig _economy;

    public int Wins => _stats.WinsValue;
    public int Losses => _stats.LossesValue;
    public int Gold => _wallet.GoldValue;

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
        _wallet.AddGold(_economy.WinGold);
        _save.SaveAll();
    }

    public void RegisterLoss()
    {
        _stats.AddLoss();
        _wallet.SubtractGoldClamped(_economy.LoseGold);
        _save.SaveAll();
    }
}