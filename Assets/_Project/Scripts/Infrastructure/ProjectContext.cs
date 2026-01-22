using UnityEngine;

public sealed class ProjectContext : MonoBehaviour
{
    public static ProjectContext Instance { get; private set; }
    public IContainer Container => container;

    private Container container;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        container = new Container();

        var argsService = new SceneArgsService();
        container.Bind(argsService);

        container.Bind(new ConfigService());
        container.Bind(new SceneLoader(argsService));
    }
}