using UnityEngine.SceneManagement;

public sealed class SceneLoader
{
    private readonly SceneArgsService args;

    public SceneLoader(SceneArgsService args)
    {
        this.args = args;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(SceneNames.MainMenu);
    }

    public void LoadGameplay(GameMode mode)
    {
        args.SetSelectedMode(mode);
        SceneManager.LoadScene(SceneNames.Gameplay);
    }

    public void ReloadCurrent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}