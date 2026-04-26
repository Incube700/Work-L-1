using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class AreaDamageTargetsSystem : IInitializableSystem, IDisposableSystem
{
    private Entity _entity;
    private float _damage;
    private AreaAttackTargets _targets;
    private SimpleEvent<Vector3> _targetsCollected;
    private SimpleEvent _attackFinished;
    private ReactiveVariable<bool> _isDead;
    private IDisposable _targetsCollectedSubscription;

    public void OnInit(Entity entity)
    {
        _entity = entity;
        _damage = entity.GetComponent<AreaAttackDamage>().Value;
        _targets = entity.GetComponent<AreaAttackTargets>();
        _targetsCollected = entity.GetComponent<AreaAttackTargetsCollected>().Value;
        _attackFinished = entity.GetComponent<AreaAttackFinished>().Value;
        _isDead = entity.IsDead;

        _targetsCollectedSubscription = _targetsCollected.Subscribe(OnTargetsCollected);
    }

    public void OnDispose()
    {
        _targetsCollectedSubscription?.Dispose();
        _targetsCollectedSubscription = null;
    }

    private void OnTargetsCollected(Vector3 position)
    {
        if (_isDead.Value)
        {
            return;
        }

        if (_damage <= 0f)
        {
            _attackFinished.Invoke();
            return;
        }

        for (int i = 0; i < _targets.Value.Count; i++)
        {
            Entity target = _targets.Value[i];

            if (CombatTargetFilter.CanDamage(_entity, target) == false)
            {
                continue;
            }

            target.TakeDamageRequest.Invoke(_damage);
        }

        _attackFinished.Invoke();
    }
}