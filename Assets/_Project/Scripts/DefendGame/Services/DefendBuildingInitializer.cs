using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;

public sealed class DefendBuildingInitializer
{
    private readonly DefendLevelConfig _level;
    private readonly DefendGameplaySceneData _sceneData;
    private readonly DefendEntitiesFactory _entitiesFactory;
    private readonly BuildingStateService _buildingStateService;
    private readonly BuildingCombatService _buildingCombatService;
    private readonly ExplosionService _explosionService;

    public DefendBuildingInitializer(
        DefendLevelConfig level,
        DefendGameplaySceneData sceneData,
        DefendEntitiesFactory entitiesFactory,
        BuildingStateService buildingStateService,
        BuildingCombatService buildingCombatService,
        ExplosionService explosionService)
    {
        _level = level ?? throw new ArgumentNullException(nameof(level));
        _sceneData = sceneData ?? throw new ArgumentNullException(nameof(sceneData));
        _entitiesFactory = entitiesFactory ?? throw new ArgumentNullException(nameof(entitiesFactory));
        _buildingStateService = buildingStateService ?? throw new ArgumentNullException(nameof(buildingStateService));
        _buildingCombatService = buildingCombatService ?? throw new ArgumentNullException(nameof(buildingCombatService));
        _explosionService = explosionService ?? throw new ArgumentNullException(nameof(explosionService));
    }

    public void Initialize()
    {
        Entity building = _entitiesFactory.CreateBuilding(_sceneData.BuildingSpawnPoint, _level);
        _buildingStateService.SetBuilding(building);

        ConstructViews(building);

        if (_buildingStateService.HasBuilding == false)
        {
            throw new InvalidOperationException("Building is not initialized.");
        }
    }

    private void ConstructViews(Entity building)
    {
        MageAttackAnimationView mageAttackAnimationView =
            building.Transform.GetComponentInChildren<MageAttackAnimationView>();

        if (mageAttackAnimationView != null)
        {
            mageAttackAnimationView.Construct(_buildingCombatService, _level, _explosionService);
        }
    }
}