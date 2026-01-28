public static class GameplayRegistrations
{
    public static void Register(IContainer container)
    {
        container.BindTransient<SequenceGenerator>(_ => new SequenceGenerator());

        container.BindTransient<GameplayLoop>(c => new GameplayLoop(
            c.Resolve<KeyboardInputReader>(),
            c.Resolve<SceneLoader>(),
            c.Resolve<ConfigService>(),
            c.Resolve<SequenceGenerator>(),
            c.Resolve<PlayerProgressService>()));
    }
}