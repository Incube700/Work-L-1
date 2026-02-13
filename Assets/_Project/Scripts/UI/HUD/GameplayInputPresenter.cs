public sealed class GameplayInputPresenter
{
    private readonly GameplayLoop _loop;
    private readonly KeyboardInputReader _input;
    private readonly GameFlowService _flow;

    public GameplayInputPresenter(GameplayLoop loop, KeyboardInputReader input, GameFlowService flow)
    {
        _loop = loop;
        _input = input;
        _flow = flow;
    }

    public void Initialize()
    {
        _input.CharTyped += OnCharTyped;
        _input.SpacePressed += OnSpacePressed;
    }

    public void Dispose()
    {
        _input.CharTyped -= OnCharTyped;
        _input.SpacePressed -= OnSpacePressed;
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