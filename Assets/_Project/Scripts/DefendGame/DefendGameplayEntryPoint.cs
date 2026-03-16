using System;
using UnityEngine;

public sealed class DefendGameplayEntryPoint : SceneEntryPointBase
{
    [SerializeField] private Transform _buildingSpawnPoint;
    [SerializeField] private LayerMask _groundMask = ~0;
    [SerializeField] private DefendGameplayScreenView _screenView;

    private DefendGameplayRuntime _runtime;

    protected override void Register(IContainer container)
    {
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

        WalletService wallet = container.Resolve<WalletService>();
        PlayerProgressService progress = container.Resolve<PlayerProgressService>();
        GameFlowService flow = container.Resolve<GameFlowService>();

        Vector3 spawnPoint = _buildingSpawnPoint != null
            ? _buildingSpawnPoint.position
            : Vector3.zero;

        _runtime = new DefendGameplayRuntime(
            args.LevelConfig,
            spawnPoint,
            _groundMask,
            wallet,
            progress,
            flow,
            _screenView);

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
    }
}
