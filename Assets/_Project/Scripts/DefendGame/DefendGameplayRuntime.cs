using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;
using Assets._Project.Scripts.Infrastructure.AssetsManagment;
using UnityEngine;

public sealed class DefendGameplayRuntime
{
    private readonly Vector3 _buildingSpawnPoint;
    private readonly EntitiesLifeContext _life;
    private readonly MonoEntitiesFactory _monoFactory;
    private readonly IInputService _input;
    private readonly DefendGameController _controller;

    private bool _isStarted;

    public DefendGameplayRuntime(
        DefendLevelConfig level,
        Vector3 buildingSpawnPoint,
        LayerMask groundMask,
        WalletService wallet,
        PlayerProgressService progress,
        GameFlowService flow)
    {
        _buildingSpawnPoint = buildingSpawnPoint;

        ResourcesAssetsLoader loader = new ResourcesAssetsLoader();

        _life = new EntitiesLifeContext();

        CollidersRegistryService collidersRegistry = new CollidersRegistryService();
        _monoFactory = new MonoEntitiesFactory(loader, _life, collidersRegistry);
        _monoFactory.Initialize();

        _input = new DesktopInputService();
        IPointerService pointer = new DesktopPointerService(Camera.main, groundMask, _buildingSpawnPoint.y);
        ExplosionService explosions = new ExplosionService(collidersRegistry);

        DefendEntitiesFactory entitiesFactory = new DefendEntitiesFactory(_life, _monoFactory);

        _controller = new DefendGameController(
            level,
            entitiesFactory,
            _life,
            _input,
            pointer,
            explosions,
            collidersRegistry,
            wallet,
            progress,
            flow);
    }

    public void Start()
    {
        if (_isStarted)
        {
            return;
        }

        _controller.Start(_buildingSpawnPoint);
        _isStarted = true;
    }

    public void Update(float deltaTime)
    {
        if (_isStarted == false)
        {
            return;
        }

        _input.Tick();
        _controller.Update(deltaTime);
        _life.Update(deltaTime);
    }

    public void Dispose()
    {
        if (_isStarted == false)
        {
            return;
        }

        _controller.Dispose();
        _life.Dispose();
        _monoFactory.Dispose();

        _isStarted = false;
    }
}
