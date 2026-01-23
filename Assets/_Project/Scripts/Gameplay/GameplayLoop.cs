using UnityEngine;

public sealed class GameplayLoop
{
    private const int SequenceLength = 6;
    private const string GameModesConfigPath = "Configs/GameModesConfig";

    private readonly KeyboardInputReader _input;
    private readonly SceneLoader _sceneLoader;
    private readonly SceneArgsService _args;
    private readonly ConfigService _configService;
    private readonly SequenceGenerator _generator;

    private TypingChecker _checker;

    private bool _isFinished;
    private bool _isWin;
    private GameMode _mode;

    public GameplayLoop(
        KeyboardInputReader input,
        SceneLoader sceneLoader,
        SceneArgsService args,
        ConfigService configService,
        SequenceGenerator generator)
    {
        this._input = input;
        this._sceneLoader = sceneLoader;
        this._args = args;
        this._configService = configService;
        this._generator = generator;
    }

    public void Start()
    {
        if (_args.TryGet<GameplayArgs>(out GameplayArgs gameplayArgs) == false)
        {
            throw new System.InvalidOperationException("GameplayArgs not found. Go to gameplay through menu.");
        }

        _mode = gameplayArgs.Mode;

        GameModesConfig modesConfig = _configService.Load<GameModesConfig>(GameModesConfigPath);
        string available = modesConfig.GetAvailableChars(_mode);

        string target = _generator.Generate(available, SequenceLength);

        Debug.Log($"MODE: {_mode}");
        Debug.Log($"TARGET: {target}");

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

        _checker.HandleChar(c);
    }

    private void OnWon()
    {
        _isFinished = true;
        _isWin = true;
        Debug.Log("WIN! Press SPACE to return to menu.");
    }

    private void OnLost()
    {
        _isFinished = true;
        _isWin = false;
        Debug.Log("LOSE! Press SPACE to restart.");
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
