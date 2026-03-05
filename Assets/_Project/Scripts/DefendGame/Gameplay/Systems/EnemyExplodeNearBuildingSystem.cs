using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class EnemyExplodeNearBuildingSystem : IInitializableSystem, IUpdatableSystem
{
    private readonly Entity _building;
    private readonly float _explodeDistance;
    private readonly float _explodeDamage;

    private Entity _self;
    private Transform _selfTransform;
    private ReactiveVariable<bool> _isDead;

    public EnemyExplodeNearBuildingSystem(Entity building, float explodeDistance, float explodeDamage)
    {
        _building = building;
        _explodeDistance = explodeDistance;
        _explodeDamage = explodeDamage;
    }

    public void OnInit(Entity entity)
    {
        _self = entity;
        _selfTransform = entity.Transform;
        _isDead = entity.IsDead;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_isDead.Value || _building == null || _building.IsDead.Value)
        {
            return;
        }

        Vector3 delta = _building.Transform.position - _selfTransform.position;
        delta.y = 0f;

        float distanceSqr = delta.sqrMagnitude;
        float explodeDistanceSqr = _explodeDistance * _explodeDistance;

        if (distanceSqr > explodeDistanceSqr)
        {
            return;
        }

        if (_building.HasComponent<TakeDamageRequest>())
        {
            _building.TakeDamageRequest.Invoke(_explodeDamage);
        }

        if (_self.HasComponent<TakeDamageRequest>())
        {
            _self.TakeDamageRequest.Invoke(_self.CurrentHealth.Value);
        }
    }
}
