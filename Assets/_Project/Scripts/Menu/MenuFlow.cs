using UnityEngine;

public sealed class MenuFlow
{
    private const char NumbersKey = '1';
    private const char LettersKey = '2';

    private readonly KeyboardInputReader _input;
    private readonly SceneLoader _sceneLoader;

    public MenuFlow(KeyboardInputReader input, SceneLoader sceneLoader)
    {
        this._input = input;
        this._sceneLoader = sceneLoader;
    }

    public void Start()
    {
        Debug.Log("MAIN MENU: 1 - Numbers, 2 - Letters");
        _input.CharTyped += OnCharTyped;
    }

    public void Stop()
    {
        _input.CharTyped -= OnCharTyped;
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
}