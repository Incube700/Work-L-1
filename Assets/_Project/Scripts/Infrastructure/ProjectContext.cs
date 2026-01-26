using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class ProjectContext : MonoBehaviour
{
    public IReadOnlyContainer Container => _container;

    private static ProjectContext _instance;

    private Container _container;
    private KeyboardInputReader _input;
    private SceneArgsService _sceneArgs;
    private SceneLoader _sceneLoader;

    private bool _isInitialized;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        Initialize();
        StartGame();
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _container = new Container();

        ProjectRegistrations.Register(_container);

        _input = _container.Resolve<KeyboardInputReader>();
        _sceneArgs = _container.Resolve<SceneArgsService>();
        _sceneLoader = _container.Resolve<SceneLoader>();

        _isInitialized = true;
    }

    private void StartGame()
    {
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_isInitialized == false)
        {
            return;
        }

        if (scene.name == SceneNames.Bootstrap)
        {
            return;
        }

        IContainer sceneContainer = _container.CreateChild();

        if (scene.name == SceneNames.MainMenu)
        {
            InitializeMainMenu(sceneContainer);
            _sceneArgs.Clear();
            return;
        }

        if (scene.name == SceneNames.Gameplay)
        {
            InitializeGameplay(sceneContainer);
            _sceneArgs.Clear();
            return;
        }

        _sceneArgs.Clear();
    }

    private void InitializeMainMenu(IContainer sceneContainer)
    {
        MainMenuEntryPoint entryPoint = FindFirstObjectByType<MainMenuEntryPoint>();

        if (entryPoint == null)
        {
            throw new InvalidOperationException("MainMenuEntryPoint not found on MainMenuScene.");
        }

        entryPoint.Initialize(sceneContainer);
    }

    private void InitializeGameplay(IContainer sceneContainer)
    {
        GameplayEntryPoint entryPoint = FindFirstObjectByType<GameplayEntryPoint>();

        if (entryPoint == null)
        {
            throw new InvalidOperationException("GameplayEntryPoint not found on GameplayScene.");
        }

        if (_sceneArgs.TryGet(out GameplayArgs args) == false)
        {
            throw new InvalidOperationException("GameplayArgs not found. Go to gameplay through menu.");
        }

        entryPoint.Initialize(sceneContainer, args);
    }
}
