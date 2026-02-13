public sealed class GameplayTargetPresenter
{
    private readonly GameplayLoop _loop;
    private readonly GameplayTargetView _view;

    public GameplayTargetPresenter(GameplayLoop loop, GameplayTargetView view)
    {
        _loop = loop;
        _view = view;
    }

    public void Initialize()
    {
        _loop.TargetChanged += OnTargetChanged;
    }

    public void Dispose()
    {
        _loop.TargetChanged -= OnTargetChanged;
    }

    private void OnTargetChanged(string target)
    {
        _view.SetTarget(target);
    }
}