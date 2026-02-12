using System;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class MainMenuEntryPoint : SceneEntryPointBase
{
    private MainMenuPresenter _presenter;
    private PopupService _popupService;

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

        PopupLayer popupLayer = Object.FindObjectOfType<PopupLayer>(true);
        if (popupLayer == null)
        {
            throw new InvalidOperationException("PopupLayer not found. Add PopupLayer to MainMenu scene (under Canvas).");
        }

        ((IContainer)container).BindInstance(view);
        ((IContainer)container).BindInstance(popupLayer);

        _popupService = container.Resolve<PopupService>();

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

        if (_popupService != null)
        {
            _popupService.Dispose();
            _popupService = null;
        }
    }
}