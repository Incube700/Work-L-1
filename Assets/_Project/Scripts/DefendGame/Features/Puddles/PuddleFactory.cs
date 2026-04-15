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

        puddle.AddSystem(new PuddleDamageSystem(
            _collidersRegistry,
            _level.PuddleConfig.Radius,
            _level.PuddleConfig.TickInterval,
            _level.PuddleConfig.DamagePerTick,
            _level.PuddleConfig.Mask));

        puddle.AddSystem(new PuddleExpireAfterWaveSystem(
            _life,
            _phaseService));

        _life.Add(puddle);
        return puddle;
    }
}