using UnityEngine;

public sealed class JsonUtilitySaveSerializer : ISaveSerializer
{
    public string Serialize<T>(T data) where T : class
    {
        return JsonUtility.ToJson(data);
    }

    public bool TryDeserialize<T>(string text, out T data) where T : class
    {
        try
        {
            data = JsonUtility.FromJson<T>(text);
            return data != null;
        }
        catch
        {
            data = null;
            return false;
        }
    }
}