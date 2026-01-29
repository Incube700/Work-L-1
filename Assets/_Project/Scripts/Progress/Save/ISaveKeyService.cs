public interface ISaveKeyService
{
    string GetKey<T>() where T : class;
}