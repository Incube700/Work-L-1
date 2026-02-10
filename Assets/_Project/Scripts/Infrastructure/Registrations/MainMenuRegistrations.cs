public static class MainMenuRegistrations
{
    public static void Register(IContainer container)
    {
        container.BindTransient<MainMenuPresenter>(c => new MainMenuPresenter(
            c.Resolve<MainMenuView>(),
            c.Resolve<SceneLoader>(),
            c.Resolve<ConfigService>(),
            c.Resolve<GameStatsService>(),
            c.Resolve<WalletService>(),
            c.Resolve<ProgressResetService>()));
    }
}