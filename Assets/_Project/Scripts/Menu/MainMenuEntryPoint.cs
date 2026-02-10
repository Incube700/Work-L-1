using System;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class MainMenuEntryPoint : SceneEntryPointBase
{
    private MainMenuPresenter _presenter;

    protected override void Register(IContainer container)
    {
        MainMenuRegistrations.Register(container);
    }

    protected override void StartScene(IReadOnlyContainer container, SceneArgsService argsService)
    {
        MainMenuView view = Object.FindObjectOfType<MainMenuView>(true);
        if (view == null)
        {
            throw new InvalidOperationException("MainMenuView not found. Add MainMenuView to MainMenu scene.");
        }

        ((IContainer)container).BindInstance(view);

        _presenter = container.Resolve<MainMenuPresenter>();
        _presenter.Start();
    }

    private void OnDestroy()
    {
        if (_presenter != null)
        {
            _presenter.Stop();
            _presenter = null;
        }
    }
}