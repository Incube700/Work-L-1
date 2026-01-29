using System;
using UnityEngine.SceneManagement;

public sealed class SceneLoader
{
    private readonly SceneArgsService _argsService;

    public event Action SceneLoaded;

    public SceneLoader(SceneArgsService argsService)
    {
        _argsService = argsService;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Load(string sceneName, IInputArgs args = null)
    {
        if (args != null)
        {
            _argsService.Set(args);
        }

        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneLoaded?.Invoke();
    }
}