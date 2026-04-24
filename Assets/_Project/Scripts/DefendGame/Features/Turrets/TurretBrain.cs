using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class TurretBrain : IInitializableSystem, IUpdatableSystem
{
    private const int BufferSize = 32;

    private readonly CollidersRegistryService _collidersRegistry;
    private readonly float _radius;
    private readonly int _mask;
    private readonly TurretAttackService _attackService;
    private readonly TurretRotateSystem _rotateSystem;
    private readonly Collider[] _buffer = new Collider[BufferSize];

    private Transform _transform;

    public TurretBrain(
        CollidersRegistryService collidersRegistry,
        float radius,
        int mask,
        TurretAttackService attackService,
        TurretRotateSystem rotateSystem)
    {
        _collidersRegistry = collidersRegistry;
        _radius = radius;
        _mask = mask;
        _attackService = attackService;
        _rotateSystem = rotateSystem;
    }

    public void OnInit(Entity entity)
    {
        _transform = entity.Transform;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_transform == null)
        {
            return;
        }

        _attackService.Tick(deltaTime);

        Entity target = FindClosestTarget();

        if (target == null)
        {
            _rotateSystem.ClearLookDirection();
            return;
        }

        Vector3 direction = target.Transform.position - _transform.position;
        direction.y = 0f;

        _rotateSystem.SetLookDirection(direction);
        _attackService.TryAttack(_transform, target, direction);
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
}