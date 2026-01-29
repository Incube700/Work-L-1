public sealed class StatsSaveProvider : ISaveProvider
{
    private readonly SaveRepository _repo;
    private readonly GameStatsService _stats;

    public StatsSaveProvider(SaveRepository repo, GameStatsService stats)
    {
        _repo = repo;
        _stats = stats;
    }

    public void Load()
    {
        if (_repo.TryLoad(out StatsData data))
        {
            _stats.Set(data.wins, data.losses);
            return;
        }

        _stats.Set(0, 0);
        Save();
    }

    public void Save()
    {
        StatsData data = new StatsData
        {
            wins = _stats.WinsValue,
            losses = _stats.LossesValue
        };

        _repo.Save(data);
    }

    public void Delete()
    {
        _repo.Delete<StatsData>();
    }
}