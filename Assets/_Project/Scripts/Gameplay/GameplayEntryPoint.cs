using UnityEngine;

public sealed class GameplayEntryPoint : MonoBehaviour
{
    private GameplayLoop _loop;

    private void Awake()
    {
        ProjectContext context = FindFirstObjectByType<ProjectContext>();

        if (context == null)
        {
            throw new System.InvalidOperationException("ProjectContext not found. Start from BootstrapScene.");
        }

        IContainer sceneContainer = context.CreateSceneContainer();

        sceneContainer.BindTransient<SequenceGenerator>(_ => new SequenceGenerator());

        sceneContainer.BindTransient<GameplayLoop>(c => new GameplayLoop(
            c.Resolve<KeyboardInputReader>(),
            c.Resolve<SceneLoader>(),
            c.Resolve<SceneArgsService>(),
            c.Resolve<ConfigService>(),
            c.Resolve<SequenceGenerator>()));

        _loop = sceneContainer.Resolve<GameplayLoop>();
    }

    private void OnEnable()
    {
        _loop.Start();
    }

    private void OnDisable()
    {
        _loop.Stop();
    }
}