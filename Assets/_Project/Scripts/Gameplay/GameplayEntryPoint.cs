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

        GameplayRegistrations.Register(sceneContainer);

        _loop = sceneContainer.Resolve<GameplayLoop>();
    }

    private void OnEnable()
    {
        _loop.Start();
    }

    private void OnDisable()
    {
        if (_loop == null)
        {
            return;
        }

        _loop.Stop();
    }
}