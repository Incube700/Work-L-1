using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

public sealed class TransformMoveByDirectionSystem : IInitializableSystem, IUpdatableSystem
{
    private Transform _transform;
    private ReactiveVariable<Vector3> _moveDirection;
    private ReactiveVariable<float> _moveSpeed;
    private ReactiveVariable<bool> _isDead;

    public void OnInit(Entity entity)
    {
        _transform = entity.Transform;
        _moveDirection = entity.MoveDirection;
        _moveSpeed = entity.MoveSpeed;
        _isDead = entity.IsDead;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_isDead.Value)
        {
            return;
        }

        Vector3 direction = _moveDirection.Value;

        if (direction.sqrMagnitude > 1f)
        {
            direction.Normalize();
        }

        _transform.position += direction * _moveSpeed.Value * deltaTime;
    }
}