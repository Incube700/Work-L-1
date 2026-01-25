public static class GameplayRegistrations
{
    public static void Register(IContainer container)
    {
        container.BindTransient<SequenceGenerator>(_ => new SequenceGenerator());

        container.BindTransient<GameplayLoop>(c => new GameplayLoop(
            c.Resolve<KeyboardInputReader>(),
            c.Resolve<SceneLoader>(),
            c.Resolve<SceneArgsService>(),
            c.Resolve<ConfigService>(),
            c.Resolve<SequenceGenerator>()));
    }
}