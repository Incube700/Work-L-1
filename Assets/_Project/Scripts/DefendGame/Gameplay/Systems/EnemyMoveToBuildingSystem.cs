using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

public sealed class EnemyMoveToBuildingSystem : IInitializableSystem, IUpdatableSystem
{
    private readonly Transform _buildingTransform;

    private Transform _selfTransform;
    private ReactiveVariable<Vector3> _moveDirection;
    private ReactiveVariable<Vector3> _rotationDirection;

    public EnemyMoveToBuildingSystem(Transform buildingTransform)
    {
        _buildingTransform = buildingTransform;
    }

    public void OnInit(Entity entity)
    {
        _selfTransform = entity.Transform;
        _moveDirection = entity.MoveDirection;
        _rotationDirection = entity.RotationDirection;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_buildingTransform == null)
        {
            return;
        }

        Vector3 dir = _buildingTransform.position - _selfTransform.position;
        dir.y = 0f;

        _moveDirection.Value = dir;
        _rotationDirection.Value = dir;
    }
}
