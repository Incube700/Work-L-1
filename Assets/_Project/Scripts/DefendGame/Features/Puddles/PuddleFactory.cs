using Assets._Project.Scripts.Gameplay.EntitiesCore;
using UnityEngine;

public sealed class PuddleFactory
{
    private readonly DefendLevelConfig _level;
    private readonly DefendEntitiesFactory _entitiesFactory;
    private readonly EntitiesLifeContext _life;
    private readonly CollidersRegistryService _collidersRegistry;
    private readonly DefendPhaseService _phaseService;

    public PuddleFactory(
        DefendLevelConfig level,
        DefendEntitiesFactory entitiesFactory,
        EntitiesLifeContext life,
        CollidersRegistryService collidersRegistry,
        DefendPhaseService phaseService)
    {
        _level = level;
        _entitiesFactory = entitiesFactory;
        _life = life;
        _collidersRegistry = collidersRegistry;
        _phaseService = phaseService;
    }

    public Entity Create(Vector3 position)
    {
        Entity puddle = _entitiesFactory.CreatePuddle(position, _level);

        PuddleTargetCollectorService targetCollector = new PuddleTargetCollectorService(
            _collidersRegistry,
            _level.PuddleConfig.Radius,
            _level.PuddleConfig.Mask);

        PuddleDamageService damageService = new PuddleDamageService(
            _level.PuddleConfig.DamagePerTick);

        puddle.AddSystem(new PuddleDamageSystem(
            targetCollector,
            damageService,
            _level.PuddleConfig.TickInterval));

        puddle.AddSystem(new PuddleExpireAfterWaveSystem(
            _life,
            _phaseService));

        _life.Add(puddle);
        return puddle;
    }
}