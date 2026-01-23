using UnityEngine;

public sealed class MainMenuEntryPoint : MonoBehaviour
{
    private MenuFlow _flow;

    private void Awake()
    {
        ProjectContext context = FindFirstObjectByType<ProjectContext>();

        if (context == null)
        {
            throw new System.InvalidOperationException("ProjectContext not found. Start from BootstrapScene.");
        }

        IContainer sceneContainer = context.CreateSceneContainer();

        sceneContainer.BindTransient<MenuFlow>(c => new MenuFlow(
            c.Resolve<KeyboardInputReader>(),
            c.Resolve<SceneLoader>()));

        _flow = sceneContainer.Resolve<MenuFlow>();
    }

    private void OnEnable()
    {
        _flow.Start();
    }
    
    private void OnDisable()
    {
        if (_flow == null)
        {
            return;
        }

        _flow.Stop();
    }

}