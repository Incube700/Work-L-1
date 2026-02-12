public static class GameplayRegistrations
{
    public static void Register(IContainer container)
    {
        container.BindTransient<SequenceGenerator>(_ => new SequenceGenerator());

        container.BindTransient<GameplayLoop>(c => new GameplayLoop(
            c.Resolve<ConfigService>(),
            c.Resolve<SequenceGenerator>(),
            c.Resolve<PlayerProgressService>()));

        container.BindTransient<GameplayPresenter>(c => new GameplayPresenter(
            c.Resolve<GameplayLoop>(),
            c.Resolve<GameplayHudView>(),
            c.Resolve<KeyboardInputReader>(),
            c.Resolve<GameFlowService>(),
            c.Resolve<WalletService>()));
    }
}