using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using UnityEngine;

public sealed class TurretRotateSystem : IInitializableSystem, IUpdatableSystem
{
    private readonly float _rotateSpeed;

    private Transform _transform;
    private Vector3 _lookDirection;

    public TurretRotateSystem(float rotateSpeed)
    {
        _rotateSpeed = rotateSpeed;
    }

    public void OnInit(Entity entity)
    {
        _transform = entity.Transform;
        _lookDirection = Vector3.zero;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_transform == null)
        {
            return;
        }

        if (_lookDirection.sqrMagnitude <= 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(_lookDirection);

        if (_rotateSpeed <= 0f)
        {
            _transform.rotation = targetRotation;
            return;
        }

        _transform.rotation = Quaternion.RotateTowards(
            _transform.rotation,
            targetRotation,
            _rotateSpeed * deltaTime);
    }

    public void SetLookDirection(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            _lookDirection = Vector3.zero;
            return;
        }

        _lookDirection = direction.normalized;
    }

    public void ClearLookDirection()
    {
        _lookDirection = Vector3.zero;
    }
}