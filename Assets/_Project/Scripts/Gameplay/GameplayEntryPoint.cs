using System;
using UnityEngine;

public sealed class GameplayEntryPoint : MonoBehaviour
{
    private GameplayLoop _loop;
    private bool _isInitialized;

    public void Initialize(IContainer sceneContainer, GameplayArgs args)
    {
        if (sceneContainer == null)
        {
            throw new ArgumentNullException(nameof(sceneContainer));
        }

        if (args == null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        if (_isInitialized)
        {
            throw new InvalidOperationException("GameplayEntryPoint is already initialized.");
        }

        GameplayRegistrations.Register(sceneContainer);

        _loop = sceneContainer.Resolve<GameplayLoop>();
        _loop.Start(args.Mode);

        _isInitialized = true;
    }

    private void OnDestroy()
    {
        if (_loop == null)
        {
            return;
        }

        _loop.Stop();
        _loop = null;
    }
}