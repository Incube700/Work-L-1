using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

public sealed class DefendEntitiesFactory
{
    private readonly EntitiesLifeContext _life;
    private readonly MonoEntitiesFactory _mono;

    public DefendEntitiesFactory(EntitiesLifeContext life, MonoEntitiesFactory mono)
    {
        _life = life;
        _mono = mono;
    }

    public Entity CreateBuilding(Vector3 position, DefendLevelConfig level)
    {
        Entity entity = new Entity();
        MonoEntity view = _mono.Create(entity, position, level.BuildingPrefabPath);

        entity.AddTransform(view.transform);

        entity.AddMaxHealth(level.BuildingHealth);
        entity.AddCurrentHealth(level.BuildingHealth);
        entity.AddIsDead(false);
        entity.AddTakeDamageRequest(new SimpleEvent<float>());

        entity.AddSystem(new ApplyDamageSystem());
        entity.AddSystem(new DeathSystem());

        _life.Add(entity);
        return entity;
    }

    public Entity CreateEnemy(Vector3 position, DefendLevelConfig level, Entity building)
    {
        Entity entity = new Entity();
        MonoEntity view = _mono.Create(entity, position, level.EnemyPrefabPath);

        entity.AddTransform(view.transform);
        entity.AddMoveDirection(Vector3.zero);
        entity.AddRotationDirection(Vector3.forward);
        entity.AddMoveSpeed(level.EnemyMoveSpeed);

        entity.AddMaxHealth(level.EnemyHealth);
        entity.AddCurrentHealth(level.EnemyHealth);
        entity.AddIsDead(false);
        entity.AddTakeDamageRequest(new SimpleEvent<float>());

        entity.AddSystem(new ApplyDamageSystem());
        entity.AddSystem(new DeathSystem());
        entity.AddSystem(new EnemyMoveToBuildingDecisionSystem(building.Transform));
        entity.AddSystem(new TransformMoveByDirectionSystem());
        entity.AddSystem(new TransformRotationSystem());
        entity.AddSystem(new EnemyExplodeNearBuildingSystem(
            building,
            level.EnemyExplodeDistance,
            level.EnemyExplodeDamage));
        entity.AddSystem(new SelfReleaseOnDeathSystem(_life));

        _life.Add(entity);
        return entity;
    }

    public Entity CreateMine(Vector3 position, DefendLevelConfig level)
    {
        Entity entity = new Entity();
        MonoEntity view = _mono.Create(entity, position, level.MinePrefabPath);

        entity.AddTransform(view.transform);

        return entity;
    }
}