public sealed class MainMenuEntryPoint : SceneEntryPointBase
{
    private MenuFlow _flow;

    protected override void Register(IContainer container)
    {
        MainMenuRegistrations.Register(container);
    }

    protected override void StartScene(IReadOnlyContainer container, SceneArgsService argsService)
    {
        _flow = container.Resolve<MenuFlow>();
        _flow.Start();
    }

    private void OnDestroy()
    {
        if (_flow != null)
        {
            _flow.Stop();
            _flow = null;
        }
    }
}