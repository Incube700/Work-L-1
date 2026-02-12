using System;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class GameplayEntryPoint : SceneEntryPointBase
{
    private GameplayPresenter _presenter;

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

        GameplayHudView hud = Object.FindObjectOfType<GameplayHudView>(true);
        if (hud == null)
        {
            throw new InvalidOperationException("GameplayHudView not found. Add GameplayHudView to Gameplay scene.");
        }

        ((IContainer)container).BindInstance(hud);

        _presenter = container.Resolve<GameplayPresenter>();
        _presenter.Start(args.Mode);
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