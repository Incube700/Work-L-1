using UnityEngine;

public sealed class GameplayLoop
{
    private const int SequenceLength = 5;

    private readonly KeyboardInputReader _input;
    private readonly SceneLoader _sceneLoader;
    private readonly ConfigService _configService;
    private readonly SequenceGenerator _generator;
    private readonly PlayerProgressService _progress;

    private TypingChecker _checker;

    private bool _isFinished;
    private bool _isWin;
    private GameMode _mode;

    public GameplayLoop(
        KeyboardInputReader input,
        SceneLoader sceneLoader,
        ConfigService configService,
        SequenceGenerator generator,
        PlayerProgressService progress)
    {
        _input = input;
        _sceneLoader = sceneLoader;
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
        if (_isFinished)
        {
            return;
        }
        
        _isFinished = true;
        _isWin = true;
        
        _progress.RegisterWin();
        
        Debug.Log($"WIN! Gold={_progress.Gold}. Press SPACE to return to menu.");
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
        
        Debug.Log($"LOSE!Gold={_progress.Gold} Press SPACE to restart.");
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
