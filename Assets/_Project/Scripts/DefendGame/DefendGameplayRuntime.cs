using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;

public sealed class DefendGameplayRuntime
{
    private readonly DefendLevelConfig _level;
    private readonly DefendGameplaySceneData _sceneData;
    private readonly EntitiesLifeContext _life;
    private readonly MonoEntitiesFactory _monoFactory;
    private readonly DefendEntitiesFactory _entitiesFactory;
    private readonly IInputService _input;
    private readonly BuildingStateService _buildingStateService;
    private readonly DefendGameController _controller;

    private bool _isStarted;

    public DefendGameplayRuntime(
        DefendLevelConfig level,
        DefendGameplaySceneData sceneData,
        EntitiesLifeContext life,
        MonoEntitiesFactory monoFactory,
        DefendEntitiesFactory entitiesFactory,
        IInputService input,
        BuildingStateService buildingStateService,
        DefendGameController controller)
    {
        _level = level;
        _sceneData = sceneData;
        _life = life;
        _monoFactory = monoFactory;
        _entitiesFactory = entitiesFactory;
        _input = input;
        _buildingStateService = buildingStateService;
        _controller = controller;
    }

    public void Start()
    {
        if (_isStarted)
        {
            return;
        }

        Entity building = _entitiesFactory.CreateBuilding(_sceneData.BuildingSpawnPoint, _level);
        _buildingStateService.SetBuilding(building);

        _controller.Start();

        if (_sceneData.ScreenView != null)
        {
            // HUD подключим следующим шагом.
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

        _isStarted = false;
    }
}