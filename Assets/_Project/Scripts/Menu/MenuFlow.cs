using UnityEngine;

public sealed class MenuFlow
{
    private const char NumbersKey = '1';
    private const char LettersKey = '2';

    private readonly KeyboardInputReader _input;
    private readonly SceneLoader _sceneLoader;
    private readonly PlayerProgressService _progress;

    public MenuFlow(KeyboardInputReader input, SceneLoader sceneLoader, PlayerProgressService progress)
    {
        _input = input;
        _sceneLoader = sceneLoader;
        _progress = progress;
    }

    public void Start()
    {
        Debug.Log("MAIN MENU: 1 - Numbers, 2 - Letters,");
        Debug.Log("MAIN MENU: F1 - Stats, F2 - Reset Progress, Ф3 - Снести сохранение.");
        _input.CharTyped += OnCharTyped;
        _input.StatsPressed += OnStatsPressed;
        _input.ResetPressed += OnResetPressed;
        _input.WipePressed += OnWipePressed;
    }

    public void Stop()
    {
        _input.CharTyped -= OnCharTyped;
        _input.StatsPressed -= OnStatsPressed;
        _input.ResetPressed -= OnResetPressed;
    }

    private void OnCharTyped(char c)
    {
        if (c == NumbersKey)
        {
            _sceneLoader.Load(SceneNames.Gameplay, new GameplayArgs(GameMode.Numbers));
        }
        else if (c == LettersKey)
        {
            _sceneLoader.Load(SceneNames.Gameplay, new GameplayArgs(GameMode.Letters));
        }
    }

    private void OnStatsPressed()
    {
        _progress.Print();
    }

    private void OnResetPressed()
    {
        _progress.TryResetProgress();
    }

    private void OnWipePressed()
    {
        _progress.WipeToDefaults();
    }
}