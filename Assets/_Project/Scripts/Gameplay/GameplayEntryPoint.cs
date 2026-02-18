using System;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class GameplayEntryPoint : SceneEntryPointBase
{
    private GameplayPresenter _presenter;
    private GameplayLoop _loop;

    protected override void Register(IContainer container)
    {
        GameplayRegistrations.Register(container);
    }

    protected override void StartScene(IReadOnlyContainer container, SceneArgsService argsService)
    {
        if (argsService.TryGet(out GameplayArgs args) == false)
        {
            throw new InvalidOperationException("GameplayArgs not found. Go to gameplay through menu.");
        }

        GameplayTargetView targetView = Object.FindObjectOfType<GameplayTargetView>(true);
        if (targetView == null)
            throw new InvalidOperationException("GameplayTargetView not found. Add it to TargetText in Gameplay scene.");

        GameplayTypedView typedView = Object.FindObjectOfType<GameplayTypedView>(true);
        if (typedView == null)
            throw new InvalidOperationException("GameplayTypedView not found. Add it to TypedText in Gameplay scene.");

        GameplayStatusView statusView = Object.FindObjectOfType<GameplayStatusView>(true);
        if (statusView == null)
            throw new InvalidOperationException("GameplayStatusView not found. Add it to StatusText in Gameplay scene.");

        CurrencyListView currencyListView = Object.FindObjectOfType<CurrencyListView>(true);
        if (currencyListView == null)
            throw new InvalidOperationException("CurrencyListView not found. Add CurrencyListView to Gameplay scene (CurrencyPanel).");

        ((IContainer)container).BindInstance(targetView);
        ((IContainer)container).BindInstance(typedView);
        ((IContainer)container).BindInstance(statusView);
        ((IContainer)container).BindInstance(currencyListView);

        // Сначала запускаем логику. UI должен быть просто подписчиком.
        _loop = container.Resolve<GameplayLoop>();
        _loop.Start(args.Mode);

        _presenter = container.Resolve<GameplayPresenter>();
        _presenter.Initialize();
    }

    private void OnDestroy()
    {
        if (_presenter != null)
        {
            // Сначала отписываем UI от событий, потом останавливаем луп.
            _presenter.Dispose();
            _loop.Stop();
            _presenter = null;
        }
    }
}