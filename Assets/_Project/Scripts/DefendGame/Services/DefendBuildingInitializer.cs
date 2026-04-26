using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;

public sealed class DefendBuildingInitializer
{
    private readonly DefendLevelConfig _level;
    private readonly DefendGameplaySceneData _sceneData;
    private readonly DefendEntitiesFactory _entitiesFactory;
    private readonly BuildingStateService _buildingStateService;
    private readonly BuildingCombatService _buildingCombatService;
    private readonly DefendPermanentUpgradesRuntime _permanentUpgradesRuntime;
    private readonly MageProjectileAttackFactory _mageProjectileAttackFactory;

    public DefendBuildingInitializer(
        DefendLevelConfig level,
        DefendGameplaySceneData sceneData,
        DefendEntitiesFactory entitiesFactory,
        BuildingStateService buildingStateService,
        BuildingCombatService buildingCombatService,
        DefendPermanentUpgradesRuntime permanentUpgradesRuntime,
        MageProjectileAttackFactory mageProjectileAttackFactory)
    {
        _level = level ?? throw new ArgumentNullException(nameof(level));
        _sceneData = sceneData ?? throw new ArgumentNullException(nameof(sceneData));
        _entitiesFactory = entitiesFactory ?? throw new ArgumentNullException(nameof(entitiesFactory));
        _buildingStateService = buildingStateService ?? throw new ArgumentNullException(nameof(buildingStateService));
        _buildingCombatService = buildingCombatService ?? throw new ArgumentNullException(nameof(buildingCombatService));
        _permanentUpgradesRuntime = permanentUpgradesRuntime ?? throw new ArgumentNullException(nameof(permanentUpgradesRuntime));
        _mageProjectileAttackFactory = mageProjectileAttackFactory ?? throw new ArgumentNullException(nameof(mageProjectileAttackFactory));
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

        if (mageAttackAnimationView == null)
        {
            return;
        }

        mageAttackAnimationView.Construct(_buildingCombatService);

        _mageProjectileAttackFactory.Create(
            mageAttackAnimationView.ProjectileSpawnPoint,
            _level.PlayerExplosionConfig.ProjectileConfig,
            _permanentUpgradesRuntime.PlayerExplosionDamageMultiplier);
    }
}