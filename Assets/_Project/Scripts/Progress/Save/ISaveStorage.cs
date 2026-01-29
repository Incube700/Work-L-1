public interface ISaveStorage
{
    bool HasKey(string key);
    string GetString(string key);
    void SetString(string key, string value);
    void DeleteKey(string key);
    void Save();
}