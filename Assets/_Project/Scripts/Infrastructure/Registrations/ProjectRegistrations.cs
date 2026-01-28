public static class ProjectRegistrations
{
    public static void Register(IContainer container)
    {
        container.BindLazy<SceneArgsService>(_ => new SceneArgsService());
        container.BindLazy<ConfigService>(_ => new ConfigService());
        container.BindLazy<KeyboardInputReader>(_ => new KeyboardInputReader());
        container.BindLazy<SceneLoader>(c => new SceneLoader(c.Resolve<SceneArgsService>()));
        
        container.BindLazy<SaveService>(_ => new SaveService());
        container.BindLazy<GameStatsService>(_ => new GameStatsService());
        container.BindLazy<WalletService>(_ => new WalletService());

        container.BindLazy<PlayerProgressService>(c => new PlayerProgressService(
            c.Resolve<SaveService>(),
            c.Resolve<ConfigService>(),
            c.Resolve<GameStatsService>(),
            c.Resolve<WalletService>()));
    }
}