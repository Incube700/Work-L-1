using System;
using UnityEngine;

public sealed class GameplayEntryPoint : SceneEntryPointBase
{
    [SerializeField] private GameplayTargetView _targetView;
    [SerializeField] private GameplayTypedView _typedView;
    [SerializeField] private GameplayStatusView _statusView;
    [SerializeField] private CurrencyListView _currencyListView;

    private GameplayPresenter _presenter;
    private GameplayLoop _loop;

    protected override void Register(IContainer container)
    {
        ValidateViews();

        container.BindInstance(_targetView);
        container.BindInstance(_typedView);
        container.BindInstance(_statusView);
        container.BindInstance(_currencyListView);

        GameplayRegistrations.Register(container);
    }

    protected override void StartScene(IReadOnlyContainer container, SceneArgsService argsService)
    {
        if (argsService.TryGet(out GameplayArgs args) == false)
        {
            throw new InvalidOperationException("Gameplay args not found. Go to gameplay through menu.");
        }

        _loop = container.Resolve<GameplayLoop>();
        _presenter = container.Resolve<GameplayPresenter>();

        _loop.Start(args.Mode);
        _presenter.Initialize();
    }

    private void OnDestroy()
    {
        if (_presenter != null)
        {
            _presenter.Dispose();
            _presenter = null;
        }

        if (_loop != null)
        {
            _loop.Stop();
            _loop = null;
        }
    }

    private void ValidateViews()
    {
        if (_targetView == null)
        {
            throw new InvalidOperationException("GameplayTargetView is not assigned.");
        }

        if (_typedView == null)
        {
            throw new InvalidOperationException("GameplayTypedView is not assigned.");
        }

        if (_statusView == null)
        {
            throw new InvalidOperationException("GameplayStatusView is not assigned.");
        }

        if (_currencyListView == null)
        {
            throw new InvalidOperationException("CurrencyListView is not assigned.");
        }
    }
}