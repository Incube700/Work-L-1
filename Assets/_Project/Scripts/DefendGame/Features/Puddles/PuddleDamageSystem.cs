using System.Collections.Generic;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class PuddleDamageSystem : IInitializableSystem, IUpdatableSystem
{
    private const int BufferSize = 32;

    private readonly CollidersRegistryService _collidersRegistry;
    private readonly float _radius;
    private readonly float _tickInterval;
    private readonly float _damagePerTick;
    private readonly int _mask;
    private readonly Collider[] _buffer = new Collider[BufferSize];
    private readonly List<Entity> _damagedTargets = new List<Entity>(BufferSize);

    private Transform _transform;
    private float _tickLeft;

    public PuddleDamageSystem(
        CollidersRegistryService collidersRegistry,
        float radius,
        float tickInterval,
        float damagePerTick,
        int mask)
    {
        _collidersRegistry = collidersRegistry;
        _radius = radius;
        _tickInterval = tickInterval;
        _damagePerTick = damagePerTick;
        _mask = mask;
    }

    public void OnInit(Entity entity)
    {
        _transform = entity.Transform;
        _tickLeft = _tickInterval;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_transform == null)
        {
            return;
        }

        _tickLeft -= deltaTime;

        if (_tickLeft > 0f)
        {
            return;
        }

        _tickLeft = _tickInterval;
        _damagedTargets.Clear();

        int count = Physics.OverlapSphereNonAlloc(
            _transform.position,
            _radius,
            _buffer,
            _mask,
            QueryTriggerInteraction.Collide);

        for (int i = 0; i < count; i++)
        {
            Collider collider = _buffer[i];

            if (collider == null)
            {
                continue;
            }

            if (_collidersRegistry.TryGetEntity(collider, out Entity target) == false)
            {
                continue;
            }

            if (_damagedTargets.Contains(target))
            {
                continue;
            }

            if (target.TryGetComponent(out TeamComponent teamComponent) == false)
            {
                continue;
            }

            if (teamComponent.Value != Team.Enemy)
            {
                continue;
            }

            if (target.HasComponent<TakeDamageRequest>() == false)
            {
                continue;
            }

            if (target.TryGetComponent(out IsDead isDead))
            {
                if (isDead.Value.Value == true)
                {
                    continue;
                }
            }
            
            target.TakeDamageRequest.Invoke(_damagePerTick);
            _damagedTargets.Add(target);
        }

        if (_damagedTargets.Count > 0)
        {
            Log($"[Defend] Puddle tick. Damaged: {_damagedTargets.Count}");
        }
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        Debug.Log(message);
    }
}