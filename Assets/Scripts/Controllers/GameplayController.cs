using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{
    private GameplayService _gameplayService;

    private void Start()
    {
        var settings = ProjectBootstrapper.SettingsService;

        if (settings != null && settings.SelectedConfig != null)
        {
            _gameplayService = new GameplayService(settings.SelectedConfig.AvailableChars, settings.SequenceLength);
        }
        else
        {
            Debug.LogError("Настройки не найдены! Запусти игру с главной сцены.");
        }
    }

    private void Update()
    {
        if (_gameplayService == null)
        {
            return;
        }

        if (_gameplayService.IsGameOver == false)
        {
            foreach (char c in Input.inputString)
            {
                _gameplayService.HandleInput(c);
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_gameplayService.IsWin)
            {
                SceneManager.LoadScene(SceneNames.MainMenu);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}