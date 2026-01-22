public sealed class SceneArgsService
{
    public GameMode SelectedMode { get; private set; }

    public void SetSelectedMode(GameMode mode)
    {
        SelectedMode = mode;
    }
}