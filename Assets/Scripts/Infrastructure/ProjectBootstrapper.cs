using UnityEngine;

public class ProjectBootstrapper : MonoBehaviour
{
    public static GameSettingsService SettingsService { get; private set; }

    [SerializeField] private int sequenceLength = 5;

    private void Awake()
    {
        if (SettingsService == null)
        {
            SettingsService = new GameSettingsService(sequenceLength);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}