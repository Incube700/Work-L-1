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

        mine.AddMaxHealth(1f);
        mine.AddCurrentHealth(1f);
        mine.AddIsDead(false);
        mine.AddTakeDamageRequest(new SimpleEvent<float>());

        mine.AddSystem(new ApplyDamageSystem());
        mine.AddSystem(new DeathSystem());
        mine.AddSystem(new MineTriggerExplosionSystem(
            _explosions,
            _colliders,
            _level.MineConfig.TriggerRadius,
            _level.MineConfig.ExplosionRadius,
            _level.MineConfig.Damage,
            _level.MineConfig.Mask));
        mine.AddSystem(new SelfReleaseOnDeathSystem(_life));

        _life.Add(mine);
        return mine;
    }
}