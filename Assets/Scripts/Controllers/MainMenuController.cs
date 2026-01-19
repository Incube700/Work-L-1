using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameConfig numbersConfig;
    [SerializeField] private GameConfig lettersConfig;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ProjectBootstrapper.SettingsService.SetConfig(numbersConfig);
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ProjectBootstrapper.SettingsService.SetConfig(lettersConfig);
            StartGame();
        }
    }

    private void StartGame()
    {
        SceneManager.LoadScene(SceneNames.Gameplay);
    }
}