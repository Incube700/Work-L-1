public static class MainMenuRegistrations
{
    public static void Register(IContainer container)
    {
        container.BindLazy<PopupService>(c => new PopupService(
            c.Resolve<ViewsFactory>(),
            c.Resolve<ProjectPresentersFactory>(),
            c.Resolve<PopupLayer>().transform));

        container.BindTransient<CurrencyListPresenter>(c => new CurrencyListPresenter(
            c.Resolve<CurrencyListView>(),
            c.Resolve<ViewsFactory>(),
            c.Resolve<WalletService>()));

        container.BindTransient<StatsPresenter>(c => new StatsPresenter(
            c.Resolve<StatsView>(),
            c.Resolve<GameStatsService>()));

        container.BindTransient<MainMenuPresenter>(c => new MainMenuPresenter(
            c.Resolve<MainMenuView>(),
            c.Resolve<GameFlowService>(),
            c.Resolve<ConfigService>(),
            c.Resolve<ProgressResetService>(),
            c.Resolve<PopupService>(),
            c.Resolve<CurrencyListPresenter>(),
            c.Resolve<StatsPresenter>()));
    }
}