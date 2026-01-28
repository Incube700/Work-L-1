public static class MainMenuRegistrations
{
    public static void Register(IContainer container)
    {
        container.BindTransient<MenuFlow>(c => new MenuFlow(
            c.Resolve<KeyboardInputReader>(),
            c.Resolve<SceneLoader>(),
            c.Resolve<PlayerProgressService>()));
    }
}