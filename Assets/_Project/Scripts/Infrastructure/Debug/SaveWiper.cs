using UnityEngine;

public sealed class SaveWiper : MonoBehaviour
{
    [ContextMenu("Delete Save Keys (Wallet + Stats)")]
    private void DeleteSaveKeys()
    {
        PlayerPrefs.DeleteKey("Save_WalletData");
        PlayerPrefs.DeleteKey("Save_StatsData");
        PlayerPrefs.Save();

        Debug.Log("Save keys deleted. Restart Play to re-init defaults.");
    }
}