public static class GameplayRegistrations
{
    public static void Register(IContainer container)
    {
        container.BindTransient<SequenceGenerator>(_ => new SequenceGenerator());

        container.BindLazy<GameplayLoop>(c => new GameplayLoop(
            c.Resolve<ConfigService>(),
            c.Resolve<SequenceGenerator>(),
            c.Resolve<PlayerProgressService>()));

        container.BindTransient<CurrencyListPresenter>(c => new CurrencyListPresenter(
            c.Resolve<CurrencyListView>(),
            c.Resolve<ViewsFactory>(),
            c.Resolve<WalletService>()));

        container.BindTransient<GameplayTargetPresenter>(c => new GameplayTargetPresenter(
            c.Resolve<GameplayLoop>(),
            c.Resolve<GameplayTargetView>()));

        container.BindTransient<GameplayTypedPresenter>(c => new GameplayTypedPresenter(
            c.Resolve<GameplayLoop>(),
            c.Resolve<GameplayTypedView>()));

        container.BindTransient<GameplayStatusPresenter>(c => new GameplayStatusPresenter(
            c.Resolve<GameplayLoop>(),
            c.Resolve<GameplayStatusView>()));

        container.BindTransient<GameplayInputPresenter>(c => new GameplayInputPresenter(
            c.Resolve<GameplayLoop>(),
            c.Resolve<KeyboardInputReader>(),
            c.Resolve<GameFlowService>()));

        container.BindTransient<GameplayPresenter>(c => new GameplayPresenter(
            c.Resolve<GameplayTargetPresenter>(),
            c.Resolve<GameplayTypedPresenter>(),
            c.Resolve<GameplayStatusPresenter>(),
            c.Resolve<GameplayInputPresenter>(),
            c.Resolve<CurrencyListPresenter>()));
    }
}