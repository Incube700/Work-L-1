using System;
using UnityEngine;

public sealed class DefendGameplayEntryPoint : SceneEntryPointBase
{
    [SerializeField] private Transform _buildingSpawnPoint;
    [SerializeField] private LayerMask _groundMask = ~0;
    [SerializeField] private DefendGameplayScreenView _screenView;
    
    private IContainer _sceneContainer;
    private DefendGameplayRuntime _runtime;

    protected override void Register(IContainer container)
    {
        _sceneContainer = container;
    }

    protected override void StartScene(IReadOnlyContainer container, SceneArgsService argsService)
    {
        if (argsService.TryGet(out DefendGameplayArgs args) == false)
        {
            throw new InvalidOperationException("DefendGameplayArgs not found.");
        }

        if (args.LevelConfig == null)
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
        

        Vector3 spawnPoint = _buildingSpawnPoint != null
            ? _buildingSpawnPoint.position
            : Vector3.zero;

        _sceneContainer.BindInstance(args.LevelConfig);
        _sceneContainer.BindInstance(_screenView.HudView);
        _sceneContainer.BindInstance(_screenView.CurrencyListView);
        _sceneContainer.BindInstance(_screenView.PopupLayer);       
        _sceneContainer.BindInstance(new DefendGameplaySceneData(
            spawnPoint,
            _groundMask));
        
        DefendGameplayRegistrations.Register(_sceneContainer);
        
        _runtime = _sceneContainer.Resolve<DefendGameplayRuntime>();
        _runtime.Start();
        
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
        if (_runtime == null)
        {
            return;
        }

        _runtime.Dispose();
        _runtime = null;
        _sceneContainer = null;
        
    }
}