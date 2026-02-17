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
        
        CurrencyListView currencyListView = Object.FindObjectOfType<CurrencyListView>(true);
        if (currencyListView == null)
        {
            throw new InvalidOperationException("CurrencyListView not found. Add CurrencyListView to MainMenu scene.");
        }
        
        StatsView statsView = Object.FindObjectOfType<StatsView>(true);
        if (statsView == null)
        {
            throw new InvalidOperationException("StatsView not found. Add StatsView to MainMenu scene.");
        }

        ((IContainer)container).BindInstance(currencyListView);
        ((IContainer)container).BindInstance(view);
        ((IContainer)container).BindInstance(popupLayer);
        ((IContainer)container).BindInstance(statsView);

        _popupService = container.Resolve<PopupService>();

        _presenter = container.Resolve<MainMenuPresenter>();
        _presenter.Initialize();
    }

    private void OnDestroy()
    {
        if (_presenter != null)
        {
            _presenter.Dispose();
            _presenter = null;
        }

        if (_popupService != null)
        {
            _popupService.Dispose();
            _popupService = null;
        }
    }
}