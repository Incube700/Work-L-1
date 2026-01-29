using System;
using UnityEngine;

public abstract class SceneEntryPointBase : MonoBehaviour
{
    private bool _isInitialized;

    public void Initialize(IContainer container, SceneArgsService argsService)
    {
        if (container == null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (argsService == null)
        {
            throw new ArgumentNullException(nameof(argsService));
        }

        if (_isInitialized)
        {
            throw new InvalidOperationException($"{GetType().Name} is already initialized.");
        }

        Register(container);
        StartScene(container, argsService);

        _isInitialized = true;
    }

    // 1) регистрация зависимостей (только bind’ы)
    protected abstract void Register(IContainer container);

    // 2) запуск (resolve + подписки/старт)
    protected abstract void StartScene(IReadOnlyContainer container, SceneArgsService argsService);
}