using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

public sealed class EnemyMoveToBuildingDecisionSystem : IInitializableSystem, IUpdatableSystem
{
    private readonly Transform _buildingTransform;

    private Transform _selfTransform;
    private ReactiveVariable<Vector3> _moveDirection;
    private ReactiveVariable<Vector3> _rotationDirection;
    private ReactiveVariable<bool> _isDead;

    public EnemyMoveToBuildingDecisionSystem(Transform buildingTransform)
    {
        _buildingTransform = buildingTransform;
    }

    public void OnInit(Entity entity)
    {
        _selfTransform = entity.Transform;
        _moveDirection = entity.MoveDirection;
        _rotationDirection = entity.RotationDirection;
        _isDead = entity.IsDead;
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

        if (_buildingTransform == null)
        {
            _moveDirection.Value = Vector3.zero;
            _rotationDirection.Value = Vector3.zero;
            return;
        }

        Vector3 direction = _buildingTransform.position - _selfTransform.position;
        direction.y = 0f;

        _moveDirection.Value = direction;
        _rotationDirection.Value = direction;
    }
}