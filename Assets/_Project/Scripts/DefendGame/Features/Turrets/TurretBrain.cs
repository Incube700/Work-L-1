using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class TurretBrain : IInitializableSystem, IUpdatableSystem
{
    private const int BufferSize = 32;

    private readonly CollidersRegistryService _collidersRegistry;
    private readonly float _radius;
    private readonly float _attackInterval;
    private readonly float _damage;
    private readonly float _impactRadius;
    private readonly int _mask;
    private readonly ExplosionService _explosionService;
    private readonly Collider[] _buffer = new Collider[BufferSize];

    private Transform _transform;
    private float _attackLeft;

    public TurretBrain(
        CollidersRegistryService collidersRegistry,
        float radius,
        float attackInterval,
        float damage,
        float impactRadius,
        int mask,
        ExplosionService explosionService)
    {
        _collidersRegistry = collidersRegistry;
        _radius = radius;
        _attackInterval = attackInterval;
        _damage = damage;
        _impactRadius = impactRadius;
        _mask = mask;
        _explosionService = explosionService;
    }

    public void OnInit(Entity entity)
    {
        _transform = entity.Transform;
        _attackLeft = _attackInterval;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_transform == null)
        {
            return;
        }

        Entity target = FindClosestTarget();

        if (target == null)
        {
            return;
        }

        RotateToTarget(target);

        _attackLeft -= deltaTime;

        if (_attackLeft > 0f)
        {
            return;
        }

        _attackLeft = _attackInterval;
        DealDamage(target);
    }

    private Entity FindClosestTarget()
    {
        int count = Physics.OverlapSphereNonAlloc(
            _transform.position,
            _radius,
            _buffer,
            _mask,
            QueryTriggerInteraction.Collide);

        Entity closestTarget = null;
        float closestDistanceSqr = float.MaxValue;

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

            float distanceSqr = (target.Transform.position - _transform.position).sqrMagnitude;

            if (distanceSqr >= closestDistanceSqr)
            {
                continue;
            }

            closestDistanceSqr = distanceSqr;
            closestTarget = target;
        }

        return closestTarget;
    }

    private void RotateToTarget(Entity target)
    {
        Vector3 direction = target.Transform.position - _transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            return;
        }

        _transform.rotation = Quaternion.LookRotation(direction);
    }

    private void DealDamage(Entity target)
    {
        target.TakeDamageRequest.Invoke(_damage);
        _explosionService.NotifyExploded(target.Transform.position, _impactRadius);

        Log($"[Defend] Turret hit target for {_damage}");
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        Debug.Log(message);
    }
}