public interface ISaveSerializer
{
    string Serialize<T>(T data) where T : class;
    bool TryDeserialize<T>(string text, out T data) where T : class;
}
