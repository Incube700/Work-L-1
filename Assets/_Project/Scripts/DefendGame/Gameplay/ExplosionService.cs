using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class ExplosionService
{
    private const int CollidersBufferSize = 128;

    private readonly CollidersRegistryService _collidersRegistry;
    private readonly Collider[] _buffer = new Collider[CollidersBufferSize];

    public event Action<Vector3, float> Exploded;

    public ExplosionService(CollidersRegistryService collidersRegistry)
    {
        _collidersRegistry = collidersRegistry;
    }

    public void Explode(Vector3 position, float radius, float damage, int mask)
    {
        if (radius <= 0f || damage <= 0f)
        {
            return;
        }

        NotifyExploded(position, radius);

        int count = Physics.OverlapSphereNonAlloc(
            position,
            radius,
            _buffer,
            mask,
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

            target.TakeDamageRequest.Invoke(damage);
        }
    }

    public void NotifyExploded(Vector3 position, float radius)
    {
        if (radius <= 0f)
        {
            return;
        }

        Exploded?.Invoke(position, radius);
    }
}