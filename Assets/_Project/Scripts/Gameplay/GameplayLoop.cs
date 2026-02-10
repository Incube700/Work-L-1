using System.Text;

public sealed partial class GameplayLoop
{
    private const int SequenceLength = 5;

    private readonly KeyboardInputReader _input;
    private readonly SceneLoader _sceneLoader;
    private readonly ConfigService _configService;
    private readonly SequenceGenerator _generator;
    private readonly PlayerProgressService _progress;
    private readonly GameplayHudView _hud;

    private TypingChecker _checker;

    private readonly StringBuilder _typed = new StringBuilder(16);

    private bool _isFinished;
    private bool _isWin;
    private GameMode _mode;

    public GameplayLoop(
        KeyboardInputReader input,
        SceneLoader sceneLoader,
        ConfigService configService,
        SequenceGenerator generator,
        PlayerProgressService progress,
        GameplayHudView hud)
    {
        _input = input;
        _sceneLoader = sceneLoader;
        _configService = configService;
        _generator = generator;
        _progress = progress;
        _hud = hud;
    }

    public void Start(GameMode mode)
    {
        _mode = mode;

        GameModesConfig modesConfig = _configService.Load<GameModesConfig>();
        string available = modesConfig.GetAvailableChars(_mode);

        string target = _generator.Generate(available, SequenceLength);

        _typed.Clear();

        _hud.SetTarget(target);
        _hud.SetTyped(string.Empty);
        _hud.SetStatus("Type the sequence.");

        _checker = new TypingChecker(target);
        _checker.Won += OnWon;
        _checker.Lost += OnLost;

        _input.CharTyped += OnCharTyped;
        _input.SpacePressed += OnSpacePressed;

        _isFinished = false;
        _isWin = false;
    }

    public void Stop()
    {
        _input.CharTyped -= OnCharTyped;
        _input.SpacePressed -= OnSpacePressed;

        if (_checker != null)
        {
            _checker.Won -= OnWon;
            _checker.Lost -= OnLost;
            _checker = null;
        }
    }

    private void OnCharTyped(char c)
    {
        if (_isFinished)
        {
            return;
        }

        char typedChar = char.ToUpperInvariant(c);
        _typed.Append(typedChar);
        _hud.SetTyped(_typed.ToString());

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

        _hud.SetStatus($"WIN! Gold={_progress.Gold}. Press SPACE to return to menu.");
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

        _hud.SetStatus($"LOSE! Gold={_progress.Gold}. Press SPACE to restart.");
    }

    private void OnSpacePressed()
    {
        if (_isFinished == false)
        {
            return;
        }

        if (_isWin)
        {
            _sceneLoader.Load(SceneNames.MainMenu);
        }
        else
        {
            _sceneLoader.Load(SceneNames.Gameplay, new GameplayArgs(_mode));
        }
    }
}