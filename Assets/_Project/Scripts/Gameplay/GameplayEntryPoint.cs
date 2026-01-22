using UnityEngine;

public sealed class GameplayEntryPoint : MonoBehaviour
{
    private const int SequenceLength = 6; // потом можно вынести в отдельный конфиг

    private KeyboardInputReader input;
    private SceneLoader sceneLoader;

    private TypingChecker checker;

    private bool isFinished;
    private bool isWin;

    private void Awake()
    {
        input = new KeyboardInputReader();
        sceneLoader = ProjectContext.Instance.Container.Resolve<SceneLoader>();

        var args = ProjectContext.Instance.Container.Resolve<SceneArgsService>();
        var configService = ProjectContext.Instance.Container.Resolve<ConfigService>();

        GameMode mode = args.SelectedMode;
        string available = configService.GameModes.GetAvailableChars(mode);

        var generator = new SequenceGenerator();
        string target = generator.Generate(available, SequenceLength);

        Debug.Log($"MODE: {mode}");
        Debug.Log($"TARGET: {target}");

        checker = new TypingChecker(target);
    }

    private void OnEnable()
    {
        input.CharTyped += OnCharTyped;
        input.SpacePressed += OnSpacePressed;

        checker.Won += OnWon;
        checker.Lost += OnLost;
    }

    private void OnDisable()
    {
        input.CharTyped -= OnCharTyped;
        input.SpacePressed -= OnSpacePressed;

        checker.Won -= OnWon;
        checker.Lost -= OnLost;
    }

    private void Update()
    {
        input.Tick();
    }

    private void OnCharTyped(char c)
    {
        if (isFinished)
        {
            return;
        }

        checker.HandleChar(c);
    }

    private void OnWon()
    {
        isFinished = true;
        isWin = true;
        Debug.Log("WIN! Press SPACE to return to menu.");
    }

    private void OnLost()
    {
        isFinished = true;
        isWin = false;
        Debug.Log("LOSE! Press SPACE to restart.");
    }

    private void OnSpacePressed()
    {
        if (isFinished == false)
        {
            return;
        }

        if (isWin)
        {
            sceneLoader.LoadMenu();
        }
        else
        {
            sceneLoader.ReloadCurrent();
        }
    }
}
