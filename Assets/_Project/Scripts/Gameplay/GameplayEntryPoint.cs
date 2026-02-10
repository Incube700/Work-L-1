using System;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class GameplayEntryPoint : SceneEntryPointBase
{
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

        GameplayHudView hud = Object.FindObjectOfType<GameplayHudView>(true);
        if (hud == null)
        {
            throw new InvalidOperationException("GameplayHudView not found. Add GameplayHudView to Gameplay scene.");
        }

        ((IContainer)container).BindInstance(hud);

        _loop = container.Resolve<GameplayLoop>();
        _loop.Start(args.Mode);
    }

    private void OnDestroy()
    {
        if (_loop != null)
        {
            _loop.Stop();
            _loop = null;
        }
    }
}