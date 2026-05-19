using System;
using UnityEngine;

public sealed class MainMenuEntryPoint : SceneEntryPointBase
{
    [SerializeField] private MainMenuScreenView _screenView;

    private IContainer _sceneContainer;
    private Container _standaloneProjectContainer;
    private IContainer _standaloneSceneContainer;
    private MainMenuPresenter _presenter;
    private PopupService _popupService;

    protected override void Register(IContainer container)
    {
        _sceneContainer = container;
        MainMenuRegistrations.Register(_sceneContainer);
    }

    protected override void StartScene(IReadOnlyContainer container, SceneArgsService argsService)
    {
        if (_screenView == null)
        {
            throw new InvalidOperationException("MainMenuScreenView is not assigned.");
        }

        if (_screenView.MainMenuView == null)
        {
            throw new InvalidOperationException("MainMenuView is not assigned in MainMenuScreenView.");
        }

        if (_screenView.PopupLayer == null)
        {
            throw new InvalidOperationException("PopupLayer is not assigned in MainMenuScreenView.");
        }

        if (_screenView.CurrencyListView == null)
        {
            throw new InvalidOperationException("CurrencyListView is not assigned in MainMenuScreenView.");
        }

        if (_screenView.StatsView == null)
        {
            throw new InvalidOperationException("StatsView is not assigned in MainMenuScreenView.");
        }

        _sceneContainer.BindInstance(_screenView.MainMenuView);
        _sceneContainer.BindInstance(_screenView.PopupLayer);
        _sceneContainer.BindInstance(_screenView.CurrencyListView);
        _sceneContainer.BindInstance(_screenView.StatsView);

        _popupService = _sceneContainer.Resolve<PopupService>();

        _presenter = _sceneContainer.Resolve<MainMenuPresenter>();
        _presenter.Initialize();
    }

    private void Start()
    {
        if (_presenter != null)
        {
            return;
        }

        InitializeStandalone();
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

        if (_standaloneSceneContainer != null)
        {
            _standaloneSceneContainer.Dispose();
            _standaloneSceneContainer = null;
        }

        if (_standaloneProjectContainer != null)
        {
            _standaloneProjectContainer.Dispose();
            _standaloneProjectContainer = null;
        }

        _sceneContainer = null;
    }

    private void InitializeStandalone()
    {
        _standaloneProjectContainer = new Container();
        ProjectRegistrations.Register(_standaloneProjectContainer);

        _standaloneProjectContainer.Resolve<SaveService>().LoadAll();

        _standaloneSceneContainer = _standaloneProjectContainer.CreateChild();
        Initialize(_standaloneSceneContainer, _standaloneProjectContainer.Resolve<SceneArgsService>());
    }
}
