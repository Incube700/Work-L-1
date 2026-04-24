using System.Collections.Generic;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class PuddleTargetCollectorService
{
    private const int BufferSize = 32;

    private readonly CollidersRegistryService _collidersRegistry;
    private readonly float _radius;
    private readonly int _mask;
    private readonly Collider[] _buffer = new Collider[BufferSize];

    public PuddleTargetCollectorService(
        CollidersRegistryService collidersRegistry,
        float radius,
        int mask)
    {
        _collidersRegistry = collidersRegistry;
        _radius = radius;
        _mask = mask;
    }

    public void Collect(Vector3 position, List<Entity> targets)
    {
        if (targets == null)
        {
            return;
        }

        targets.Clear();

        int count = Physics.OverlapSphereNonAlloc(
            position,
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

            if (targets.Contains(target))
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

            if (target.TryGetComponent(out IsDead isDead))
            {
                if (isDead.Value.Value == true)
                {
                    continue;
                }
            }

            if (target.HasComponent<TakeDamageRequest>() == false)
            {
                continue;
            }

            targets.Add(target);
        }
    }
}