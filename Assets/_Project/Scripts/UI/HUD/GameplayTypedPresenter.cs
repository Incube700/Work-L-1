public sealed class GameplayTypedPresenter
{
    private readonly GameplayLoop _loop;
    private readonly GameplayTypedView _view;

    public GameplayTypedPresenter(GameplayLoop loop, GameplayTypedView view)
    {
        _loop = loop;
        _view = view;
    }

    public void Initialize()
    {
        _loop.TargetChanged += OnTargetChanged;
        _loop.TypedChanged += OnTypedChanged;
        OnTypedChanged(_loop.Typed);
    }

    public void Dispose()
    {
        _loop.TargetChanged -= OnTargetChanged;
        _loop.TypedChanged -= OnTypedChanged;
    }

    private void OnTargetChanged(string _)
    {
        _view.SetTyped(string.Empty);
    }

    private void OnTypedChanged(string typed)
    {
        _view.SetTyped(typed);
    }
}