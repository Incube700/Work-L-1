using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneLoader : IDisposable
{
    private readonly SceneArgsService _argsService;
    private bool _isLoading;
    private bool _isDisposed;

    public event Action SceneLoaded;

    public SceneLoader(SceneArgsService argsService)
    {
        _argsService = argsService;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Load(string sceneName, IInputArgs args = null)
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(nameof(SceneLoader));
        }

        if (_isLoading)
        {
            Debug.LogWarning($"Scene load ignored. Previous load is still in progress. Target: {sceneName}");
            return;
        }

        if (args != null)
        {
            _argsService.Set(args);
        }

        _isLoading = true;

        LoadingOverlay overlay = LoadingOverlay.Create();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        if (operation == null)
        {
            _isLoading = false;
            overlay.Dispose();
            throw new InvalidOperationException($"Failed to start async scene load for '{sceneName}'.");
        }

        overlay.Bind(operation);

        operation.completed += _ =>
        {
            _isLoading = false;
            overlay.Dispose();
        };
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
        _isDisposed = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneLoaded?.Invoke();
    }
}