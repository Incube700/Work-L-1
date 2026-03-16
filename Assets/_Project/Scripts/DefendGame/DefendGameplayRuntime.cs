using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;
using Assets._Project.Scripts.Infrastructure.AssetsManagment;
using UnityEngine;

public sealed class DefendGameplayRuntime
{
    private readonly DefendLevelConfig _level;
    private readonly Vector3 _buildingSpawnPoint;
    private readonly LayerMask _groundMask;
    private readonly WalletService _wallet;
    private readonly PlayerProgressService _progress;
    private readonly GameFlowService _flow;
    private readonly DefendGameplayScreenView _screenView;

    private EntitiesLifeContext _life;
    private CollidersRegistryService _collidersRegistry;
    private MonoEntitiesFactory _monoFactory;
    private DefendEntitiesFactory _entitiesFactory;
    private IInputService _input;
    private IPointerService _pointer;
    private ExplosionService _explosions;
    private DefendGameController _controller;

    private bool _isStarted;

    public DefendGameplayRuntime(
        DefendLevelConfig level,
        Vector3 buildingSpawnPoint,
        LayerMask groundMask,
        WalletService wallet,
        PlayerProgressService progress,
        GameFlowService flow,
        DefendGameplayScreenView screenView)
    {
        _level = level;
        _buildingSpawnPoint = buildingSpawnPoint;
        _groundMask = groundMask;
        _wallet = wallet;
        _progress = progress;
        _flow = flow;
        _screenView = screenView;
    }

    public void Start()
    {
        if (_isStarted)
        {
            return;
        }

        ResourcesAssetsLoader loader = new ResourcesAssetsLoader();

        _life = new EntitiesLifeContext();

        _collidersRegistry = new CollidersRegistryService();
        _monoFactory = new MonoEntitiesFactory(loader, _life, _collidersRegistry);
        _monoFactory.Initialize();

        _input = new DesktopInputService();
        _pointer = new DesktopPointerService(Camera.main, _groundMask, _buildingSpawnPoint.y);
        _explosions = new ExplosionService(_collidersRegistry);

        _entitiesFactory = new DefendEntitiesFactory(_life, _monoFactory);

        _controller = new DefendGameController(
            _level,
            _entitiesFactory,
            _life,
            _input,
            _pointer,
            _explosions,
            _collidersRegistry,
            _wallet,
            _progress,
            _flow);

        _controller.Start(_buildingSpawnPoint);

        // ScreenView пока просто прокинут в runtime.
        // Следующим шагом подключим HudPresenter.
        if (_screenView != null)
        {
        }

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

        _controller = null;
        _life = null;
        _collidersRegistry = null;
        _monoFactory = null;
        _entitiesFactory = null;
        _input = null;
        _pointer = null;
        _explosions = null;

        _isStarted = false;
    }
}
