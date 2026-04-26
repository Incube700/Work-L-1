using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class AreaAttackFeedbackSystem : IInitializableSystem, IDisposableSystem
{
    private readonly ExplosionService _explosionService;

    private float _radius;
    private bool _shouldShowExplosion;
    private SimpleEvent<Vector3> _attackRequest;
    private ReactiveVariable<bool> _isDead;
    private IDisposable _attackRequestSubscription;

    public AreaAttackFeedbackSystem(ExplosionService explosionService)
    {
        _explosionService = explosionService;
    }

    public void OnInit(Entity entity)
    {
        _radius = entity.GetComponent<AreaAttackRadius>().Value;
        _shouldShowExplosion = entity.GetComponent<AreaAttackShouldShowExplosion>().Value;
        _attackRequest = entity.GetComponent<AreaAttackRequest>().Value;
        _isDead = entity.IsDead;

        _attackRequestSubscription = _attackRequest.Subscribe(OnAttackRequested);
    }

    public void OnDispose()
    {
        _attackRequestSubscription?.Dispose();
        _attackRequestSubscription = null;
    }

    private void OnAttackRequested(Vector3 position)
    {
        if (_isDead.Value)
        {
            return;
        }

        if (_shouldShowExplosion == false)
        {
            return;
        }

        _explosionService.NotifyExploded(position, _radius);
    }
}