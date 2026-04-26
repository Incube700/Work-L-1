using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

public sealed class TurretFactory
{
    private readonly DefendLevelConfig _level;
    private readonly DefendEntitiesFactory _entitiesFactory;
    private readonly EntitiesLifeContext _life;
    private readonly CollidersRegistryService _collidersRegistry;
    private readonly ProjectileFactory _projectileFactory;

    public TurretFactory(
        DefendLevelConfig level,
        DefendEntitiesFactory entitiesFactory,
        EntitiesLifeContext life,
        CollidersRegistryService collidersRegistry,
        ProjectileFactory projectileFactory)
    {
        _level = level;
        _entitiesFactory = entitiesFactory;
        _life = life;
        _collidersRegistry = collidersRegistry;
        _projectileFactory = projectileFactory;
    }

    public Entity Create(Vector3 position)
    {
        Entity turret = _entitiesFactory.CreateTurret(position, _level);

        turret.AddComponent(new ProjectileShootRequest { Value = new SimpleEvent<Vector3>() });
        turret.AddComponent(new ProjectileShootInterval { Value = _level.TurretConfig.AttackInterval });
        turret.AddComponent(new ProjectileShootCooldown { Value = new ReactiveVariable<float>(0f) });
        turret.AddComponent(new ProjectileShootConfig { Value = _level.TurretConfig.ProjectileConfig });

        turret.AddSystem(new ProjectileShootSystem(_projectileFactory));

        turret.AddSystem(new TurretBrain(
            _collidersRegistry,
            _level.TurretConfig.Radius,
            _level.TurretConfig.Mask));

        turret.AddSystem(new TransformRotationSystem());

        _life.Add(turret);
        return turret;
    }
}