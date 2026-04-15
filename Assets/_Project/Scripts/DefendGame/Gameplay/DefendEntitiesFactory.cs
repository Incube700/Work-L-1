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

    public DefendEntitiesFactory(
        EntitiesLifeContext life,
        MonoEntitiesFactory mono,
        ExplosionService explosionService)
    {
        _life = life;
        _mono = mono;
        _explosionService = explosionService;
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

    public Entity CreateEnemy(Vector3 position, DefendLevelConfig level, WaveConfig wave, Entity building)
{
    switch (wave.EnemyType)
    {
        case EnemyType.Bomber:
            return CreateBomberEnemy(position, level, building);

        case EnemyType.Shooter:
            return CreateShooterEnemy(position, level, building);

        default:
            throw new System.ArgumentOutOfRangeException();
    }
}

private Entity CreateBomberEnemy(Vector3 position, DefendLevelConfig level, Entity building)
{
    Entity entity = new Entity();
    MonoEntity view = _mono.Create(entity, position, level.EnemyConfig.PrefabPath);

    entity.AddTransform(view.transform);
    entity.AddComponent(new TeamComponent(Team.Enemy));

    entity.AddMoveDirection(Vector3.zero);
    entity.AddRotationDirection(Vector3.forward);
    entity.AddMoveSpeed(level.EnemyConfig.MoveSpeed);

    entity.AddMaxHealth(level.EnemyConfig.Health);
    entity.AddCurrentHealth(level.EnemyConfig.Health);
    entity.AddIsDead(false);
    entity.AddTakeDamageRequest(new SimpleEvent<float>());

    entity.AddSystem(new ApplyDamageSystem());
    entity.AddSystem(new DeathSystem());
    entity.AddSystem(new EnemyMoveToBuildingDecisionSystem(building.Transform));
    entity.AddSystem(new TransformMoveByDirectionSystem());
    entity.AddSystem(new TransformRotationSystem());
    entity.AddSystem(new EnemyExplodeNearBuildingSystem(
        building,
        level.EnemyConfig.ExplodeDistance,
        level.EnemyConfig.ExplodeDamage,
        _explosionService));
    entity.AddSystem(new ReleaseAfterDeathDelaySystem(_life, 1.2f));

    _life.Add(entity);
    return entity;
}

private Entity CreateShooterEnemy(Vector3 position, DefendLevelConfig level, Entity building)
{
    Entity entity = new Entity();
    MonoEntity view = _mono.Create(entity, position, level.ShooterEnemyConfig.PrefabPath);

    entity.AddTransform(view.transform);
    entity.AddComponent(new TeamComponent(Team.Enemy));

    entity.AddMoveDirection(Vector3.zero);
    entity.AddRotationDirection(Vector3.forward);
    entity.AddMoveSpeed(level.ShooterEnemyConfig.MoveSpeed);

    entity.AddMaxHealth(level.ShooterEnemyConfig.Health);
    entity.AddCurrentHealth(level.ShooterEnemyConfig.Health);
    entity.AddIsDead(false);
    entity.AddTakeDamageRequest(new SimpleEvent<float>());

    entity.AddSystem(new ApplyDamageSystem());
    entity.AddSystem(new DeathSystem());
    entity.AddSystem(new ShooterEnemyBrain(
        building,
        level.ShooterEnemyConfig.AttackDistance,
        level.ShooterEnemyConfig.AttackInterval,
        level.ShooterEnemyConfig.AttackDamage,
        level.ShooterEnemyConfig.ImpactRadius,
        _explosionService));
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