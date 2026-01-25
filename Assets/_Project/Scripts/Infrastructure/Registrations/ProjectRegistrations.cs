public static class ProjectRegistrations
{
    public static void Register(IContainer container)
    {
        container.BindLazy<SceneArgsService>(_ => new SceneArgsService());
        container.BindLazy<ConfigService>(_ => new ConfigService());
        container.BindLazy<KeyboardInputReader>(_ => new KeyboardInputReader());
        container.BindLazy<SceneLoader>(c => new SceneLoader(c.Resolve<SceneArgsService>()));
    }
}