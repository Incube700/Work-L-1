using System;
using UnityEngine;

public sealed class ProjectContext : MonoBehaviour
{
    private Container _container;

    private KeyboardInputReader _input;
    private SceneLoader _sceneLoader;
    private SceneArgsService _argsService;
    private SaveService _saveService;

    private bool _isInitialized;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Initialize();
        _sceneLoader.Load(SceneNames.MainMenu);
    }

    private void Update()
    {
        if (_isInitialized == false)
        {
            return;
        }

        _input.Tick();
    }

    private void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _container = new Container();

        ProjectRegistrations.Register(_container);

        _input = _container.Resolve<KeyboardInputReader>();
        _sceneLoader = _container.Resolve<SceneLoader>();
        _argsService = _container.Resolve<SceneArgsService>();
        _saveService = _container.Resolve<SaveService>();

        // чтобы дефолты были видны сразу в меню
        _saveService.LoadAll();

        _sceneLoader.SceneLoaded += OnSceneLoaded;

        _isInitialized = true;
    }

    private void OnDestroy()
    {
        if (_sceneLoader != null)
        {
            _sceneLoader.SceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded()
    {
        IContainer sceneContainer = _container.CreateChild();

        SceneEntryPointBase entryPoint = UnityEngine.Object.FindFirstObjectByType<SceneEntryPointBase>();

        if (entryPoint == null)
        {
            throw new InvalidOperationException("SceneEntryPointBase not found on loaded scene.");
        }

        entryPoint.Initialize(sceneContainer, _argsService);

        _argsService.Clear();
    }
}