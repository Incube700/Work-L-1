public sealed class SaveRepository
{
    private readonly ISaveStorage _storage;
    private readonly ISaveSerializer _serializer;
    private readonly ISaveKeyService _keys;

    public SaveRepository(ISaveStorage storage, ISaveSerializer serializer, ISaveKeyService keys)
    {
        _storage = storage;
        _serializer = serializer;
        _keys = keys;
    }

    public bool TryLoad<T>(out T data) where T : class
    {
        string key = _keys.GetKey<T>();

        if (_storage.HasKey(key) == false)
        {
            data = null;
            return false;
        }

        string text = _storage.GetString(key);
        return _serializer.TryDeserialize(text, out data);
    }

    public void Save<T>(T data) where T : class
    {
        string key = _keys.GetKey<T>();
        string text = _serializer.Serialize(data);

        _storage.SetString(key, text);
        _storage.Save();
    }

    public void Delete<T>() where T : class
    {
        string key = _keys.GetKey<T>();
        _storage.DeleteKey(key);
        _storage.Save();
    }
}