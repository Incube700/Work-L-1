using System;
using UnityEngine;

public sealed class DefendGameplayEntryPoint : SceneEntryPointBase
{
    [SerializeField] private Transform _buildingSpawnPoint;
    [SerializeField] private LayerMask _groundMask = ~0;
    [SerializeField] private DefendGameplayScreenView _screenView;

    private IContainer _sceneContainer;
    private Container _standaloneProjectContainer;
    private IContainer _standaloneSceneContainer;
    private DefendGameplayRuntime _runtime;

    protected override void Register(IContainer container)
    {
        _sceneContainer = container;
    }

    protected override void StartScene(IReadOnlyContainer container, SceneArgsService argsService)
    {
        DefendLevelConfig levelConfig = ResolveLevelConfig(container, argsService);

        if (levelConfig == null)
        {
            throw new InvalidOperationException("Defend level config is null.");
        }

        if (_screenView == null)
        {
            throw new InvalidOperationException("DefendGameplayScreenView is not assigned.");
        }

        if (_screenView.HudView == null)
        {
            throw new InvalidOperationException("DefendHudView is not assigned in DefendGameplayScreenView.");
        }

        if (_screenView.PopupLayer == null)
        {
            throw new InvalidOperationException("PopupLayer is not assigned in DefendGameplayScreenView.");
        }

        if (_screenView.CurrencyListView == null)
        {
            throw new InvalidOperationException("CurrencyListView is not assigned in DefendGameplayScreenView.");
        }

        if (_screenView.PlacementPanelView == null)
        {
            throw new InvalidOperationException("PlacementPanelView is not assigned in DefendGameplayScreenView.");
        }


        Vector3 spawnPoint = _buildingSpawnPoint != null
            ? _buildingSpawnPoint.position
            : Vector3.zero;

        _sceneContainer.BindInstance(levelConfig);
        _sceneContainer.BindInstance(_screenView);
        _sceneContainer.BindInstance(_screenView.HudView);
        _sceneContainer.BindInstance(_screenView.PlacementPanelView);
        _sceneContainer.BindInstance(_screenView.CurrencyListView);
        _sceneContainer.BindInstance(_screenView.PopupLayer);
        _sceneContainer.BindInstance(new DefendGameplaySceneData(
            spawnPoint,
            _groundMask));

        DefendGameplayRegistrations.Register(_sceneContainer);

        _runtime = _sceneContainer.Resolve<DefendGameplayRuntime>();
        _runtime.Start();

    }

    private void Start()
    {
        if (_runtime != null)
        {
            return;
        }

        InitializeStandalone();
    }

    private void Update()
    {
        if (_runtime == null)
        {
            return;
        }

        _runtime.Update(Time.deltaTime);
    }

    private void OnDestroy()
    {
        if (_runtime != null)
        {
            _runtime.Dispose();
            _runtime = null;
        }

        if (_standaloneSceneContainer != null)
        {
            _standaloneSceneContainer.Dispose();
            _standaloneSceneContainer = null;
        }

        if (_standaloneProjectContainer != null)
        {
            _standaloneProjectContainer.Dispose();
            _standaloneProjectContainer = null;
        }

        _sceneContainer = null;

    }

    private void InitializeStandalone()
    {
        _standaloneProjectContainer = new Container();
        ProjectRegistrations.Register(_standaloneProjectContainer);

        _standaloneProjectContainer.Resolve<SaveService>().LoadAll();

        _standaloneSceneContainer = _standaloneProjectContainer.CreateChild();
        Initialize(_standaloneSceneContainer, _standaloneProjectContainer.Resolve<SceneArgsService>());
    }

    private DefendLevelConfig ResolveLevelConfig(
        IReadOnlyContainer container,
        SceneArgsService argsService)
    {
        if (argsService.TryGet(out DefendGameplayArgs args))
        {
            return args.LevelConfig;
        }

        DefendLevelsConfig levelsConfig = container
            .Resolve<ConfigService>()
            .Load<DefendLevelsConfig>();

        if (levelsConfig.Levels == null || levelsConfig.Levels.Count == 0)
        {
            throw new InvalidOperationException("DefendLevelsConfig has no levels.");
        }

        Debug.Log("[Defend] No scene args found. Starting GameplayScene directly with the first configured defend level.");
        return levelsConfig.Levels[0];
    }
}
