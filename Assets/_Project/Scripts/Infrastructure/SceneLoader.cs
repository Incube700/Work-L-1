using UnityEngine.SceneManagement;

public sealed class SceneLoader
{
    private readonly SceneArgsService _args;

    public SceneLoader(SceneArgsService args)
    {
        this._args = args;
    }

    public void Load(string sceneName)
    {
        _args.Clear();
        SceneManager.LoadScene(sceneName);
    }

    public void Load(string sceneName, IInputArgs inputArgs)
    {
        _args.Set(inputArgs);
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}