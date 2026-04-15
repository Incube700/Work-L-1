using Assets._Project.Scripts.Gameplay.EntitiesCore;
using UnityEngine;

public sealed class TurretFactory
{
    private readonly DefendLevelConfig _level;
    private readonly DefendEntitiesFactory _entitiesFactory;
    private readonly EntitiesLifeContext _life;
    private readonly CollidersRegistryService _collidersRegistry;
    private readonly ExplosionService _explosionService;

    public TurretFactory(
        DefendLevelConfig level,
        DefendEntitiesFactory entitiesFactory,
        EntitiesLifeContext life,
        CollidersRegistryService collidersRegistry,
        ExplosionService explosionService)
    {
        _level = level;
        _entitiesFactory = entitiesFactory;
        _life = life;
        _collidersRegistry = collidersRegistry;
        _explosionService = explosionService;
    }

    public Entity Create(Vector3 position)
    {
        Entity turret = _entitiesFactory.CreateTurret(position, _level);

        turret.AddSystem(new TurretBrain(
            _collidersRegistry,
            _level.TurretConfig.Radius,
            _level.TurretConfig.AttackInterval,
            _level.TurretConfig.Damage,
            _level.TurretConfig.ImpactRadius,
            _level.TurretConfig.Mask,
            _explosionService));

        _life.Add(turret);
        return turret;
    }
}