using System.Collections.Generic;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
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

        puddle.AddIsDead(false);

        puddle.AddComponent(new TimedAreaAttackInterval { Value = _level.PuddleConfig.TickInterval });
        puddle.AddComponent(new TimedAreaAttackLeft { Value = new ReactiveVariable<float>(_level.PuddleConfig.TickInterval) });

        puddle.AddComponent(new AreaAttackRadius { Value = _level.PuddleConfig.Radius });
        puddle.AddComponent(new AreaAttackDamage { Value = _level.PuddleConfig.DamagePerTick });
        puddle.AddComponent(new AreaAttackMask { Value = _level.PuddleConfig.Mask });
        puddle.AddComponent(new AreaAttackRequest { Value = new SimpleEvent<Vector3>() });
        puddle.AddComponent(new AreaAttackTargets { Value = new List<Entity>() });
        puddle.AddComponent(new AreaAttackTargetsCollected { Value = new SimpleEvent<Vector3>() });
        puddle.AddComponent(new AreaAttackFinished { Value = new SimpleEvent() });
        puddle.AddComponent(new AreaAttackShouldShowExplosion { Value = false });

        puddle.AddSystem(new TimedAreaAttackRequestSystem());
        puddle.AddSystem(new AreaAttackTargetsCollectSystem(_collidersRegistry));
        puddle.AddSystem(new AreaDamageTargetsSystem());

        puddle.AddSystem(new PuddleExpireAfterWaveSystem(
            _life,
            _phaseService));

        _life.Add(puddle);
        return puddle;
    }
}