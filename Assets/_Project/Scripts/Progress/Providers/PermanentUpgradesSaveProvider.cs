public sealed class PermanentUpgradesSaveProvider : ISaveProvider
{
    private readonly SaveRepository _repo;
    private readonly PermanentUpgradesService _service;

    public PermanentUpgradesSaveProvider(SaveRepository repo, PermanentUpgradesService service)
    {
        _repo = repo;
        _service = service;
    }

    public void Load()
    {
        if (_repo.TryLoad(out PermanentUpgradesData data))
        {
            _service.Load(data);
            return;
        }

        _service.Load(new PermanentUpgradesData());
        Save();
    }

    public void Save()
    {
        _repo.Save(_service.CreateSnapshot());
    }

    public void Delete()
    {
        _repo.Delete<PermanentUpgradesData>();
    }
}
