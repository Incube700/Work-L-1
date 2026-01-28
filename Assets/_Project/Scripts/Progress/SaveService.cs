using UnityEngine;

public sealed class SaveService
{
    private const string SaveKey = "TypingGame_Save";

    public bool TryLoad(out SaveData data)
    {
        if (PlayerPrefs.HasKey(SaveKey) == false)
        {
            data = null;
            return false;
        }

        string json = PlayerPrefs.GetString(SaveKey);

        if (string.IsNullOrEmpty(json))
        {
            data = null;
            return false;
        }

        try
        {
            data = JsonUtility.FromJson<SaveData>(json);
        }
        catch
        {
            // Битое — удаляем чтобы не крашить игру.
            Delete();
            data = null;
            return false;
        }

        if (data == null)
        {
            Delete();
            return false;
        }

        return true;
    }

    public void Save(SaveData data)
    {
        if (data == null)
        {
            throw new System.ArgumentNullException(nameof(data));
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public void Delete()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
    }
}