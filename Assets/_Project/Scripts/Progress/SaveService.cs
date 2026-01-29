public sealed class SaveService
{
    private readonly ISaveProvider[] _providers;

    public SaveService(ISaveProvider[] providers)
    {
        _providers = providers;
    }

    public void LoadAll()
    {
        for (int i = 0; i < _providers.Length; i++)
        {
            _providers[i].Load();
        }
    }

    public void SaveAll()
    {
        for (int i = 0; i < _providers.Length; i++)
        {
            _providers[i].Save();
        }
    }

    public void DeleteAll()
    {
        for (int i = 0; i < _providers.Length; i++)
        {
            _providers[i].Delete();
        }
    }
}