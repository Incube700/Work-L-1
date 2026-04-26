using System.Collections.Generic;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class MineFactory
{
    private readonly DefendLevelConfig _level;
    private readonly DefendEntitiesFactory _entitiesFactory;
    private readonly EntitiesLifeContext _life;
    private readonly ExplosionService _explosions;
    private readonly CollidersRegistryService _colliders;

    public MineFactory(
        DefendLevelConfig level,
        DefendEntitiesFactory entitiesFactory,
        EntitiesLifeContext life,
        ExplosionService explosions,
        CollidersRegistryService colliders)
    {
        _level = level;
        _entitiesFactory = entitiesFactory;
        _life = life;
        _explosions = explosions;
        _colliders = colliders;
    }

    public Entity Create(Vector3 position)
    {
        Entity mine = _entitiesFactory.CreateMine(position, _level);

        mine.AddIsDead(false);

        mine.AddComponent(new OverlapActivationRadius { Value = _level.MineConfig.TriggerRadius });
        mine.AddComponent(new OverlapActivationMask { Value = _level.MineConfig.Mask });

        mine.AddComponent(new AreaAttackRadius { Value = _level.MineConfig.ExplosionRadius });
        mine.AddComponent(new AreaAttackDamage { Value = _level.MineConfig.Damage });
        mine.AddComponent(new AreaAttackMask { Value = _level.MineConfig.Mask });
        mine.AddComponent(new AreaAttackRequest { Value = new SimpleEvent<Vector3>() });
        mine.AddComponent(new AreaAttackTargets { Value = new List<Entity>() });
        mine.AddComponent(new AreaAttackTargetsCollected { Value = new SimpleEvent<Vector3>() });
        mine.AddComponent(new AreaAttackFinished { Value = new SimpleEvent() });
        mine.AddComponent(new AreaAttackShouldShowExplosion { Value = true });

        mine.AddSystem(new OverlapAttackActivationSystem(_colliders));
        mine.AddSystem(new AreaAttackFeedbackSystem(_explosions));
        mine.AddSystem(new AreaAttackTargetsCollectSystem(_colliders));
        mine.AddSystem(new AreaDamageTargetsSystem());
        mine.AddSystem(new AreaAttackSelfReleaseSystem());
        mine.AddSystem(new SelfReleaseOnDeathSystem(_life));

        _life.Add(mine);
        return mine;
    }
}