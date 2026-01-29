using System;

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