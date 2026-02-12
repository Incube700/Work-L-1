public sealed class GameFlowService
{
    private readonly SceneLoader _sceneLoader;

    public GameFlowService(SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    public void OpenMainMenu()
    {
        _sceneLoader.Load(SceneNames.MainMenu);
    }

    public void OpenGameplay(GameMode mode)
    {
        _sceneLoader.Load(SceneNames.Gameplay, new GameplayArgs(mode));
    }
}