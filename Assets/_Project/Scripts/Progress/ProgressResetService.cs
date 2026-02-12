public sealed class ProgressResetService
{
    private readonly GameStatsService _stats;
    private readonly WalletService _wallet;
    private readonly ConfigService _configs;
    private readonly SaveService _save;

    public ProgressResetService(GameStatsService stats, WalletService wallet, ConfigService configs, SaveService save)
    {
        _stats = stats;
        _wallet = wallet;
        _configs = configs;
        _save = save;
    }

    public bool TryResetStats(out string failReason)
    {
        EconomyConfig economy = _configs.Load<EconomyConfig>();
        int cost = economy.ResetCost;

        if (_wallet.TrySpend(CurrencyType.Gold, cost) == false)
        {
            int have = _wallet.Get(CurrencyType.Gold);
            failReason = $"Not enough gold. Need {cost}, have {have}.";
            return false;
        }

        _stats.Reset();
        _save.SaveAll();

        failReason = null;
        return true;
    }
}