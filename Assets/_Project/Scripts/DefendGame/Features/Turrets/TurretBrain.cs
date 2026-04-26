using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

public sealed class TurretBrain : IInitializableSystem, IUpdatableSystem
{
    private const int BufferSize = 32;

    private readonly CollidersRegistryService _collidersRegistry;
    private readonly float _radius;
    private readonly int _mask;
    private readonly Collider[] _buffer = new Collider[BufferSize];

    private Transform _transform;
    private ReactiveVariable<Vector3> _rotationDirection;
    private SimpleEvent<Vector3> _shootRequest;

    public TurretBrain(
        CollidersRegistryService collidersRegistry,
        float radius,
        int mask)
    {
        _collidersRegistry = collidersRegistry;
        _radius = radius;
        _mask = mask;
    }

    public void OnInit(Entity entity)
    {
        _transform = entity.Transform;
        _rotationDirection = entity.RotationDirection;
        _shootRequest = entity.GetComponent<ProjectileShootRequest>().Value;
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
            _rotationDirection.Value = Vector3.zero;
            return;
        }

        Vector3 direction = target.Transform.position - _transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            _rotationDirection.Value = Vector3.zero;
            return;
        }

        _rotationDirection.Value = direction;
        _shootRequest.Invoke(target.Transform.position);
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

            if (CombatTargetFilter.CanDamageFromTeam(Team.Player, target) == false)
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