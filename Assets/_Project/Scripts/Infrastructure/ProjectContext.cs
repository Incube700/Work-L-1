using UnityEngine;

public sealed class ProjectContext : MonoBehaviour
{
    public IReadOnlyContainer Container => _container;

    private Container _container;
    private KeyboardInputReader _input;
    private bool _isInitialized;

    private void Awake()
    {
        ProjectContext[] contexts = FindObjectsByType<ProjectContext>(FindObjectsSortMode.None);

        if (contexts.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _container = new Container();

        _container.BindLazy<SceneArgsService>(_ => new SceneArgsService());
        _container.BindLazy<ConfigService>(_ => new ConfigService());
        _container.BindLazy<KeyboardInputReader>(_ => new KeyboardInputReader());
        _container.BindLazy<SceneLoader>(c => new SceneLoader(c.Resolve<SceneArgsService>()));

        _input = _container.Resolve<KeyboardInputReader>();

        _isInitialized = true;
    }

    public IContainer CreateSceneContainer()
    {
        if (_isInitialized == false)
        {
            throw new System.InvalidOperationException("ProjectContext not initialized. Start from BootstrapScene.");
        }

        return _container.CreateChild();
    }

    private void Update()
    {
        if (_isInitialized == false)
        {
            return;
        }

        _input.Tick();
    }
}