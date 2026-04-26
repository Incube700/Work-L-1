using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

public sealed class ShooterEnemyBrain : IInitializableSystem, IUpdatableSystem
{
    private readonly Entity _building;
    private readonly float _attackDistance;

    private Transform _selfTransform;
    private ReactiveVariable<Vector3> _moveDirection;
    private ReactiveVariable<Vector3> _rotationDirection;
    private ReactiveVariable<bool> _isDead;
    private SimpleEvent<Vector3> _shootRequest;

    public ShooterEnemyBrain(
        Entity building,
        float attackDistance)
    {
        _building = building;
        _attackDistance = attackDistance;
    }

    public void OnInit(Entity entity)
    {
        _selfTransform = entity.Transform;
        _moveDirection = entity.MoveDirection;
        _rotationDirection = entity.RotationDirection;
        _isDead = entity.IsDead;
        _shootRequest = entity.GetComponent<ProjectileShootRequest>().Value;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_selfTransform == null)
        {
            return;
        }

        if (_isDead.Value)
        {
            Stop();
            return;
        }

        if (_building == null || _building.IsDead.Value)
        {
            Stop();
            return;
        }

        Vector3 direction = _building.Transform.position - _selfTransform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.0001f)
        {
            Stop();
            return;
        }

        _rotationDirection.Value = direction;

        if (direction.sqrMagnitude > _attackDistance * _attackDistance)
        {
            _moveDirection.Value = direction;
            return;
        }

        _moveDirection.Value = Vector3.zero;
        _shootRequest.Invoke(_building.Transform.position);
    }

    private void Stop()
    {
        _moveDirection.Value = Vector3.zero;
        _rotationDirection.Value = Vector3.zero;
    }
}