using System;
using System.Text;

public sealed partial class GameplayLoop
{
    private const int SequenceLength = 5;

    public event Action<string> TargetChanged;
    public event Action<string> TypedChanged;
    public event Action<bool> Finished; // true = win, false = lose

    private readonly ConfigService _configService;
    private readonly SequenceGenerator _generator;
    private readonly PlayerProgressService _progress;

    private TypingChecker _checker;

    private readonly StringBuilder _typed = new StringBuilder(16);

    private bool _isFinished;
    private bool _isWin;
    private GameMode _mode;

    public bool IsFinished => _isFinished;
    public bool IsWin => _isWin;
    public GameMode Mode => _mode;

    public GameplayLoop(
        ConfigService configService,
        SequenceGenerator generator,
        PlayerProgressService progress)
    {
        _configService = configService;
        _generator = generator;
        _progress = progress;
    }

    public void Start(GameMode mode)
    {
        _mode = mode;

        GameModesConfig modesConfig = _configService.Load<GameModesConfig>();
        string available = modesConfig.GetAvailableChars(_mode);

        string target = _generator.Generate(available, SequenceLength);

        _typed.Clear();

        TargetChanged?.Invoke(target);
        TypedChanged?.Invoke(string.Empty);

        _checker = new TypingChecker(target);
        _checker.Won += OnWon;
        _checker.Lost += OnLost;

        _isFinished = false;
        _isWin = false;
    }

    public void Stop()
    {
        if (_checker != null)
        {
            _checker.Won -= OnWon;
            _checker.Lost -= OnLost;
            _checker = null;
        }
    }

    public void HandleChar(char c)
    {
        if (_isFinished)
        {
            return;
        }

        char typedChar = char.ToUpperInvariant(c);
        _typed.Append(typedChar);

        TypedChanged?.Invoke(_typed.ToString());

        _checker.HandleChar(c);
    }

    private void OnWon()
    {
        if (_isFinished)
        {
            return;
        }

        _isFinished = true;
        _isWin = true;

        _progress.RegisterWin();

        Finished?.Invoke(true);
    }

    private void OnLost()
    {
        if (_isFinished)
        {
            return;
        }

        _isFinished = true;
        _isWin = false;

        _progress.RegisterLoss();

        Finished?.Invoke(false);
    }
}