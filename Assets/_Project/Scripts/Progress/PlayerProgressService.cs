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
        RegisterWin(_economy.WinGold, 0);
    }

    public void RegisterWin(int rewardGold)
    {
        RegisterWin(rewardGold, 0);
    }

    public void RegisterWin(int rewardGold, int rewardDiamonds)
    {
        if (rewardGold < 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(rewardGold), "Win gold reward must be >= 0.");
        }

        if (rewardDiamonds < 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(rewardDiamonds), "Win diamond reward must be >= 0.");
        }

        if (rewardGold == 0 && rewardDiamonds == 0)
        {
            throw new System.ArgumentOutOfRangeException("Win reward must contain at least one positive value.");
        }

        _stats.AddWin();

        if (rewardGold > 0)
        {
            _wallet.Add(CurrencyType.Gold, rewardGold);
        }

        if (rewardDiamonds > 0)
        {
            _wallet.Add(CurrencyType.Diamond, rewardDiamonds);
        }

        _save.SaveAll();
    }

    public void RegisterLoss()
    {
        _stats.AddLoss();
        _save.SaveAll();
    }
}