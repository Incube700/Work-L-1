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

    public void OpenDefendGameplay(DefendLevelConfig level)
    {
        if (level == null)
        {
            throw new System.ArgumentNullException(nameof(level));
        }

        _sceneLoader.Load(SceneNames.DefendGameplay, new DefendGameplayArgs(level));
    }
}
