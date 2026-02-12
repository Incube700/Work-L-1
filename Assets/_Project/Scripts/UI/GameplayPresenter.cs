public sealed class GameplayPresenter
{
    private readonly GameplayLoop _loop;
    private readonly GameplayHudView _view;
    private readonly KeyboardInputReader _input;
    private readonly GameFlowService _flow;
    private readonly WalletService _wallet;

    public GameplayPresenter(
        GameplayLoop loop,
        GameplayHudView view,
        KeyboardInputReader input,
        GameFlowService flow,
        WalletService wallet)
    {
        _loop = loop;
        _view = view;
        _input = input;
        _flow = flow;
        _wallet = wallet;
    }

    public void Start(GameMode mode)
    {
        _loop.TargetChanged += OnTargetChanged;
        _loop.TypedChanged += OnTypedChanged;
        _loop.Finished += OnFinished;

        _input.CharTyped += OnCharTyped;
        _input.SpacePressed += OnSpacePressed;

        _view.SetStatus("Type the sequence.");
        _loop.Start(mode);
    }

    public void Stop()
    {
        _input.CharTyped -= OnCharTyped;
        _input.SpacePressed -= OnSpacePressed;

        _loop.TargetChanged -= OnTargetChanged;
        _loop.TypedChanged -= OnTypedChanged;
        _loop.Finished -= OnFinished;

        _loop.Stop();
    }

    private void OnTargetChanged(string target)
    {
        _view.SetTarget(target);
        _view.SetTyped(string.Empty);
    }

    private void OnTypedChanged(string typed)
    {
        _view.SetTyped(typed);
    }

    private void OnFinished(bool isWin)
    {
        int gold = _wallet.Get(CurrencyType.Gold);

        if (isWin)
        {
            _view.SetStatus($"WIN! Gold={gold}. Press SPACE to return to menu.");
        }
        else
        {
            _view.SetStatus($"LOSE! Gold={gold}. Press SPACE to restart.");
        }
    }

    private void OnCharTyped(char c)
    {
        _loop.HandleChar(c);
    }

    private void OnSpacePressed()
    {
        if (_loop.IsFinished == false)
        {
            return;
        }

        if (_loop.IsWin)
        {
            _flow.OpenMainMenu();
        }
        else
        {
            _flow.OpenGameplay(_loop.Mode);
        }
    }
}