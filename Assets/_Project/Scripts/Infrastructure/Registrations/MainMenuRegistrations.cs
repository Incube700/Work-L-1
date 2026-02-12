public static class MainMenuRegistrations
{
    public static void Register(IContainer container)
    {
        container.BindLazy<PopupService>(c => new PopupService(
            c.Resolve<ViewsFactory>(),
            c.Resolve<ProjectPresentersFactory>(),
            c.Resolve<PopupLayer>().transform));

        container.BindTransient<MainMenuPresenter>(c => new MainMenuPresenter(
            c.Resolve<MainMenuView>(),
            c.Resolve<GameFlowService>(),
            c.Resolve<ConfigService>(),
            c.Resolve<GameStatsService>(),
            c.Resolve<WalletService>(),
            c.Resolve<ProgressResetService>(),
            c.Resolve<PopupService>()));
    }
}