public sealed class GameplayStatusPresenter
{
    private readonly GameplayLoop _loop;
    private readonly GameplayStatusView _view;

    public GameplayStatusPresenter(GameplayLoop loop, GameplayStatusView view)
    {
        _loop = loop;
        _view = view;
    }

    public void Initialize()
    {
        _loop.Finished += OnFinished;
        _view.SetStatus("Type the sequence.");
    }

    public void Dispose()
    {
        _loop.Finished -= OnFinished;
    }

    private void OnFinished(bool isWin)
    {
        if (isWin)
        {
            _view.SetStatus("WIN! Press SPACE to return to menu.");
        }
        else
        {
            _view.SetStatus("LOSE! Press SPACE to restart.");
        }
    }
}