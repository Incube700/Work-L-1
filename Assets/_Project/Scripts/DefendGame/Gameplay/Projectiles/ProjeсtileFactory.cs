using System;
using System.Collections.Generic;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

public sealed class ProjectileFactory
{
    private readonly MonoEntitiesFactory _monoFactory;
    private readonly EntitiesLifeContext _life;
    private readonly CollidersRegistryService _collidersRegistry;
    private readonly ExplosionService _explosionService;

    public ProjectileFactory(
        MonoEntitiesFactory monoFactory,
        EntitiesLifeContext life,
        CollidersRegistryService collidersRegistry,
        ExplosionService explosionService)
    {
        _monoFactory = monoFactory ?? throw new ArgumentNullException(nameof(monoFactory));
        _life = life ?? throw new ArgumentNullException(nameof(life));
        _collidersRegistry = collidersRegistry ?? throw new ArgumentNullException(nameof(collidersRegistry));
        _explosionService = explosionService ?? throw new ArgumentNullException(nameof(explosionService));
    }

    public Entity Create(
        Vector3 position,
        Vector3 direction,
        Vector3 targetPoint,
        Team team,
        ProjectileConfig config,
        float damageMultiplier = 1f)
    {
        if (config == null)
        {
            throw new ArgumentNullException(nameof(config));
        }

        Vector3 normalizedDirection = direction;
        normalizedDirection.y = 0f;

        if (normalizedDirection.sqrMagnitude <= 0.001f)
        {
            normalizedDirection = Vector3.forward;
        }
        else
        {
            normalizedDirection.Normalize();
        }

        Entity projectile = new Entity();
        MonoEntity view = _monoFactory.Create(projectile, position, config.PrefabPath);

        view.transform.rotation = Quaternion.LookRotation(normalizedDirection);

        projectile.AddTransform(view.transform);
        projectile.AddComponent(new TeamComponent(team));

        projectile.AddMoveDirection(normalizedDirection);
        projectile.AddMoveSpeed(config.Speed);
        projectile.AddIsDead(false);

        projectile.AddComponent(new ProjectileHitDistance { Value = config.HitDistance });
        projectile.AddComponent(new ProjectileLifetime { Value = new ReactiveVariable<float>(config.LifeTime) });
        projectile.AddComponent(new ProjectileTargetPoint { Value = targetPoint });

        projectile.AddComponent(new OverlapActivationRadius { Value = config.CollisionRadius });
        projectile.AddComponent(new OverlapActivationMask { Value = config.Mask });

        projectile.AddComponent(new AreaAttackRadius { Value = config.ExplosionRadius });
        projectile.AddComponent(new AreaAttackDamage
        {
            Value = config.Damage * Mathf.Max(0f, damageMultiplier)
        });
        projectile.AddComponent(new AreaAttackMask { Value = config.Mask });
        projectile.AddComponent(new AreaAttackRequest { Value = new SimpleEvent<Vector3>() });
        projectile.AddComponent(new AreaAttackTargets { Value = new List<Entity>() });
        projectile.AddComponent(new AreaAttackTargetsCollected { Value = new SimpleEvent<Vector3>() });
        projectile.AddComponent(new AreaAttackFinished { Value = new SimpleEvent() });
        projectile.AddComponent(new AreaAttackShouldShowExplosion { Value = true });

        projectile.AddSystem(new TransformMoveByDirectionSystem());
        projectile.AddSystem(new OverlapAttackActivationSystem(_collidersRegistry));
        projectile.AddSystem(new ProjectileReachTargetPointSystem());
        projectile.AddSystem(new ProjectileLifetimeSystem());

        projectile.AddSystem(new AreaAttackFeedbackSystem(_explosionService));
        projectile.AddSystem(new AreaAttackTargetsCollectSystem(_collidersRegistry));
        projectile.AddSystem(new AreaDamageTargetsSystem());
        projectile.AddSystem(new AreaAttackSelfReleaseSystem());

        projectile.AddSystem(new SelfReleaseOnDeathSystem(_life));

        _life.Add(projectile);

        return projectile;
    }
}