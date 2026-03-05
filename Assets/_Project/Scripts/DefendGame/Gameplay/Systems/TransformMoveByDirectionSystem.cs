using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

public sealed class TransformMoveByDirectionSystem : IInitializableSystem, IUpdatableSystem
{
    private Transform _transform;
    private ReactiveVariable<Vector3> _moveDirection;
    private ReactiveVariable<float> _moveSpeed;

    public void OnInit(Entity entity)
    {
        _transform = entity.Transform;
        _moveDirection = entity.MoveDirection;
        _moveSpeed = entity.MoveSpeed;
    }

    public void OnUpdate(float deltaTime)
    {
        Vector3 dir = _moveDirection.Value;

        if (dir.sqrMagnitude > 1f)
        {
            dir.Normalize();
        }

        _transform.position += dir * _moveSpeed.Value * deltaTime;
    }
}
