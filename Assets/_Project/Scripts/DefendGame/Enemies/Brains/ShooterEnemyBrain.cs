using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

public sealed class ShooterEnemyBrain : IInitializableSystem, IUpdatableSystem
{
    private readonly Entity _building;
    private readonly float _attackDistance;
    private readonly float _attackInterval;
    private readonly float _attackDamage;
    private readonly float _impactRadius;
    private readonly string _projectilePrefabPath;
    private readonly ShooterProjectileService _projectileService;

    private Transform _selfTransform;
    private ReactiveVariable<Vector3> _moveDirection;
    private ReactiveVariable<Vector3> _rotationDirection;
    private ReactiveVariable<bool> _isDead;
    private float _attackLeft;

    public ShooterEnemyBrain(
        Entity building,
        float attackDistance,
        float attackInterval,
        float attackDamage,
        float impactRadius,
        string projectilePrefabPath,
        ShooterProjectileService projectileService)
    {
        _building = building;
        _attackDistance = attackDistance;
        _attackInterval = attackInterval;
        _attackDamage = attackDamage;
        _impactRadius = impactRadius;
        _projectilePrefabPath = projectilePrefabPath;
        _projectileService = projectileService;
    }

    public void OnInit(Entity entity)
    {
        _selfTransform = entity.Transform;
        _moveDirection = entity.MoveDirection;
        _rotationDirection = entity.RotationDirection;
        _isDead = entity.IsDead;
        _attackLeft = _attackInterval;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_selfTransform == null)
        {
            return;
        }

        if (_isDead.Value)
        {
            _moveDirection.Value = Vector3.zero;
            _rotationDirection.Value = Vector3.zero;
            return;
        }

        if (_building == null || _building.IsDead.Value)
        {
            _moveDirection.Value = Vector3.zero;
            _rotationDirection.Value = Vector3.zero;
            return;
        }

        Vector3 direction = _building.Transform.position - _selfTransform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.0001f)
        {
            _moveDirection.Value = Vector3.zero;
            _rotationDirection.Value = Vector3.zero;
            return;
        }

        _rotationDirection.Value = direction;

        float attackDistanceSqr = _attackDistance * _attackDistance;
        float currentDistanceSqr = direction.sqrMagnitude;

        if (currentDistanceSqr > attackDistanceSqr)
        {
            _moveDirection.Value = direction;
            return;
        }

        _moveDirection.Value = Vector3.zero;

        _attackLeft -= deltaTime;

        if (_attackLeft > 0f)
        {
            return;
        }

        _attackLeft = _attackInterval;
        Shoot(direction);
    }

    private void Shoot(Vector3 direction)
    {
        _projectileService.Spawn(
            _selfTransform,
            direction,
            _building,
            _projectilePrefabPath,
            _attackDamage,
            _impactRadius);
    }
}