using UnityEngine;

public sealed class MainMenuEntryPoint : MonoBehaviour
{
    private KeyboardInputReader input;
    private SceneLoader sceneLoader;

    private void Awake()
    {
        input = new KeyboardInputReader();
        sceneLoader = ProjectContext.Instance.Container.Resolve<SceneLoader>();

        Debug.Log("MAIN MENU: 1 - Numbers, 2 - Letters");
    }

    private void OnEnable()
    {
        input.CharTyped += OnCharTyped;
    }

    private void OnDisable()
    {
        input.CharTyped -= OnCharTyped;
    }

    private void Update()
    {
        input.Tick();
    }

    private void OnCharTyped(char c)
    {
        if (c == '1')
        {
            sceneLoader.LoadGameplay(GameMode.Numbers);
        }
        else if (c == '2')
        {
            sceneLoader.LoadGameplay(GameMode.Letters);
        }
    }
}