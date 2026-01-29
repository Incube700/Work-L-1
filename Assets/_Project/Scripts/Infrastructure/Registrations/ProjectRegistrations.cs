public static class ProjectRegistrations
{
    public static void Register(IContainer container)
    {
        container.BindLazy<SceneArgsService>(_ => new SceneArgsService());
        container.BindLazy<KeyboardInputReader>(_ => new KeyboardInputReader());

        container.BindLazy<ConfigService>(_ => new ConfigService());

        container.BindLazy<SceneLoader>(c => new SceneLoader(
            c.Resolve<SceneArgsService>()));

        // save pipeline
        container.BindLazy<ISaveStorage>(_ => new PlayerPrefsSaveStorage());
        container.BindLazy<ISaveSerializer>(_ => new JsonUtilitySaveSerializer());
        container.BindLazy<ISaveKeyService>(_ => new SaveKeyService());

        container.BindLazy<SaveRepository>(c => new SaveRepository(
            c.Resolve<ISaveStorage>(),
            c.Resolve<ISaveSerializer>(),
            c.Resolve<ISaveKeyService>()));

        container.BindLazy<GameStatsService>(_ => new GameStatsService());
        container.BindLazy<WalletService>(_ => new WalletService());

        container.BindLazy<StatsSaveProvider>(c => new StatsSaveProvider(
            c.Resolve<SaveRepository>(),
            c.Resolve<GameStatsService>()));

        container.BindLazy<WalletSaveProvider>(c => new WalletSaveProvider(
            c.Resolve<SaveRepository>(),
            c.Resolve<WalletService>(),
            c.Resolve<ConfigService>()));

        container.BindLazy<SaveService>(c => new SaveService(new ISaveProvider[]
        {
            c.Resolve<StatsSaveProvider>(),
            c.Resolve<WalletSaveProvider>()
        }));

        container.BindLazy<PlayerProgressService>(c => new PlayerProgressService(
            c.Resolve<GameStatsService>(),
            c.Resolve<WalletService>(),
            c.Resolve<ConfigService>(),
            c.Resolve<SaveService>()));

        container.BindLazy<ProgressResetService>(c => new ProgressResetService(
            c.Resolve<GameStatsService>(),
            c.Resolve<WalletService>(),
            c.Resolve<ConfigService>(),
            c.Resolve<SaveService>()));
    }
}
