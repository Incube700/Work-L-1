using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class ProjectileReachTargetPointSystem : IInitializableSystem, IUpdatableSystem
{
    private Transform _transform;
    private Vector3 _targetPoint;
    private float _hitDistance;
    private SimpleEvent<Vector3> _attackRequest;
    private ReactiveVariable<bool> _isDead;

    public void OnInit(Entity entity)
    {
        _transform = entity.Transform;
        _targetPoint = entity.GetComponent<ProjectileTargetPoint>().Value;
        _hitDistance = entity.GetComponent<ProjectileHitDistance>().Value;
        _attackRequest = entity.GetComponent<AreaAttackRequest>().Value;
        _isDead = entity.IsDead;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_isDead.Value)
        {
            return;
        }

        Vector3 currentPoint = _transform.position;
        currentPoint.y = 0f;

        Vector3 targetPoint = _targetPoint;
        targetPoint.y = 0f;

        if (Vector3.Distance(currentPoint, targetPoint) > _hitDistance)
        {
            return;
        }

        _attackRequest.Invoke(_targetPoint);
    }
}