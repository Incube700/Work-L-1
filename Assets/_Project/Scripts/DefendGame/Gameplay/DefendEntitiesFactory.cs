using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

public sealed class DefendEntitiesFactory
{
    private readonly EntitiesLifeContext _life;
    private readonly MonoEntitiesFactory _mono;
    private readonly ExplosionService _explosionService;
    private readonly ShooterProjectileService _shooterProjectileService;

    public DefendEntitiesFactory(
        EntitiesLifeContext life,
        MonoEntitiesFactory mono,
        ExplosionService explosionService,
        ShooterProjectileService shooterProjectileService)
    {
        _life = life;
        _mono = mono;
        _explosionService = explosionService;
        _shooterProjectileService = shooterProjectileService;
    }

    public Entity CreateBuilding(Vector3 position, DefendLevelConfig level)
    {
        Entity entity = new Entity();
        MonoEntity view = _mono.Create(entity, position, level.BuildingConfig.PrefabPath);

        entity.AddTransform(view.transform);
        entity.AddComponent(new TeamComponent(Team.Player));
        entity.AddRotationDirection(Vector3.forward);

        entity.AddMaxHealth(level.BuildingConfig.Health);
        entity.AddCurrentHealth(level.BuildingConfig.Health);
        entity.AddIsDead(false);
        entity.AddTakeDamageRequest(new SimpleEvent<float>());

        entity.AddSystem(new ApplyDamageSystem());
        entity.AddSystem(new DeathSystem());

        _life.Add(entity);
        return entity;
    }

    public Entity CreateEnemy(Vector3 position, EnemyConfigBase enemyConfig, Entity building)
    {
        if (enemyConfig == null)
        {
            throw new System.InvalidOperationException("Enemy config is null.");
        }

        switch (enemyConfig)
        {
            case EnemyConfig bomberConfig:
                return CreateBomberEnemy(position, bomberConfig, building);

            case ShooterEnemyConfig shooterConfig:
                return CreateShooterEnemy(position, shooterConfig, building);

            default:
                throw new System.ArgumentOutOfRangeException(
                    nameof(enemyConfig),
                    enemyConfig.GetType().Name,
                    "Unsupported enemy config type.");
        }
    }

    private Entity CreateBomberEnemy(Vector3 position, EnemyConfig config, Entity building)
    {
        Entity entity = new Entity();
        MonoEntity view = _mono.Create(entity, position, config.PrefabPath);

        entity.AddTransform(view.transform);
        entity.AddComponent(new TeamComponent(Team.Enemy));

        entity.AddMoveDirection(Vector3.zero);
        entity.AddRotationDirection(Vector3.forward);
        entity.AddMoveSpeed(config.MoveSpeed);

        entity.AddMaxHealth(config.Health);
        entity.AddCurrentHealth(config.Health);
        entity.AddIsDead(false);
        entity.AddTakeDamageRequest(new SimpleEvent<float>());

        entity.AddSystem(new ApplyDamageSystem());
        entity.AddSystem(new DeathSystem());
        entity.AddSystem(new EnemyMoveToBuildingDecisionSystem(building.Transform));
        entity.AddSystem(new TransformMoveByDirectionSystem());
        entity.AddSystem(new TransformRotationSystem());
        entity.AddSystem(new EnemyExplodeNearBuildingSystem(
            building,
            config.ExplodeDistance,
            config.ExplodeDamage,
            _explosionService));
        entity.AddSystem(new ReleaseAfterDeathDelaySystem(_life, 1.2f));

        _life.Add(entity);
        return entity;
    }

    private Entity CreateShooterEnemy(Vector3 position, ShooterEnemyConfig config, Entity building)
    {
        Entity entity = new Entity();
        MonoEntity view = _mono.Create(entity, position, config.PrefabPath);

        entity.AddTransform(view.transform);
        entity.AddComponent(new TeamComponent(Team.Enemy));

        entity.AddMoveDirection(Vector3.zero);
        entity.AddRotationDirection(Vector3.forward);
        entity.AddMoveSpeed(config.MoveSpeed);

        entity.AddMaxHealth(config.Health);
        entity.AddCurrentHealth(config.Health);
        entity.AddIsDead(false);
        entity.AddTakeDamageRequest(new SimpleEvent<float>());

        entity.AddSystem(new ApplyDamageSystem());
        entity.AddSystem(new DeathSystem());
        entity.AddSystem(new ShooterEnemyBrain(
            building,
            config.AttackDistance,
            config.AttackInterval,
            config.AttackDamage,
            config.ImpactRadius,
            config.ProjectilePrefabPath,
            _shooterProjectileService));
        entity.AddSystem(new TransformMoveByDirectionSystem());
        entity.AddSystem(new TransformRotationSystem());
        entity.AddSystem(new ReleaseAfterDeathDelaySystem(_life, 1.2f));

        _life.Add(entity);
        return entity;
    }

    public Entity CreateMine(Vector3 position, DefendLevelConfig level)
    {
        Entity entity = new Entity();
        MonoEntity view = _mono.Create(entity, position, level.MineConfig.PrefabPath);

        entity.AddTransform(view.transform);
        entity.AddComponent(new TeamComponent(Team.Player));

        return entity;
    }

    public Entity CreateTurret(Vector3 position, DefendLevelConfig level)
    {
        Entity entity = new Entity();
        MonoEntity view = _mono.Create(entity, position, level.TurretConfig.PrefabPath);

        entity.AddTransform(view.transform);
        entity.AddComponent(new TeamComponent(Team.Player));
        entity.AddRotationDirection(Vector3.forward);

        return entity;
    }

    public Entity CreatePuddle(Vector3 position, DefendLevelConfig level)
    {
        Entity entity = new Entity();
        MonoEntity view = _mono.Create(entity, position, level.PuddleConfig.PrefabPath);

        entity.AddTransform(view.transform);
        entity.AddComponent(new TeamComponent(Team.Player));

        return entity;
    }
}