using System;
using UnityEngine;

public sealed class MainMenuEntryPoint : MonoBehaviour
{
    private MenuFlow _flow;
    private bool _isInitialized;

    public void Initialize(IContainer sceneContainer)
    {
        if (sceneContainer == null)
        {
            throw new ArgumentNullException(nameof(sceneContainer));
        }

        if (_isInitialized)
        {
            throw new InvalidOperationException("MainMenuEntryPoint is already initialized.");
        }

        MainMenuRegistrations.Register(sceneContainer);

        _flow = sceneContainer.Resolve<MenuFlow>();
        _flow.Start();

        _isInitialized = true;
    }

    private void OnDestroy()
    {
        if (_flow == null)
        {
            return;
        }

        _flow.Stop();
        _flow = null;
    }
}