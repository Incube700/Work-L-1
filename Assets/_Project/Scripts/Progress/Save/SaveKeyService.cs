public sealed class SaveKeyService : ISaveKeyService
{
    private const string Prefix = "Save_";

    public string GetKey<T>() where T : class
    {
        return Prefix + typeof(T).Name;
    }
}