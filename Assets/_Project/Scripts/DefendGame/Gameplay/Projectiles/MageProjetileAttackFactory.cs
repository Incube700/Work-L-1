using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class MageProjectileAttackFactory
{
    private readonly EntitiesLifeContext _life;
    private readonly ProjectileFactory _projectileFactory;
    private readonly BuildingCombatService _buildingCombatService;

    public MageProjectileAttackFactory(
        EntitiesLifeContext life,
        ProjectileFactory projectileFactory,
        BuildingCombatService buildingCombatService)
    {
        _life = life;
        _projectileFactory = projectileFactory;
        _buildingCombatService = buildingCombatService;
    }

    public Entity Create(
        Transform projectileSpawnPoint,
        ProjectileConfig projectileConfig,
        float damageMultiplier)
    {
        Entity entity = new Entity();

        entity.AddTransform(projectileSpawnPoint);
        entity.AddComponent(new TeamComponent(Team.Player));

        entity.AddComponent(new ProjectileShootRequest { Value = new SimpleEvent<Vector3>() });
        entity.AddComponent(new ProjectileShootInterval { Value = 0f });
        entity.AddComponent(new ProjectileShootCooldown { Value = new ReactiveVariable<float>(0f) });
        entity.AddComponent(new ProjectileShootConfig { Value = projectileConfig });
        entity.AddComponent(new ProjectileShootDamageMultiplier { Value = damageMultiplier });

        entity.AddSystem(new BuildingCombatProjectileRequestSystem(_buildingCombatService));
        entity.AddSystem(new ProjectileShootSystem(_projectileFactory));

        _life.Add(entity);
        return entity;
    }
}