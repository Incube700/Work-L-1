using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class AreaAttackTargetsCollectSystem : IInitializableSystem, IDisposableSystem
{
    private const int BufferSize = 64;

    private readonly CollidersRegistryService _collidersRegistry;
    private readonly Collider[] _buffer = new Collider[BufferSize];

    private Entity _entity;
    private float _radius;
    private int _mask;
    private AreaAttackTargets _targets;
    private SimpleEvent<Vector3> _attackRequest;
    private SimpleEvent<Vector3> _targetsCollected;
    private ReactiveVariable<bool> _isDead;
    private IDisposable _attackRequestSubscription;

    public AreaAttackTargetsCollectSystem(CollidersRegistryService collidersRegistry)
    {
        _collidersRegistry = collidersRegistry;
    }

    public void OnInit(Entity entity)
    {
        _entity = entity;
        _radius = entity.GetComponent<AreaAttackRadius>().Value;
        _mask = entity.GetComponent<AreaAttackMask>().Value;
        _targets = entity.GetComponent<AreaAttackTargets>();
        _attackRequest = entity.GetComponent<AreaAttackRequest>().Value;
        _targetsCollected = entity.GetComponent<AreaAttackTargetsCollected>().Value;
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

        _targets.Value.Clear();

        int count = Physics.OverlapSphereNonAlloc(
            position,
            _radius,
            _buffer,
            _mask,
            QueryTriggerInteraction.Collide);

        for (int i = 0; i < count; i++)
        {
            Collider collider = _buffer[i];

            if (collider == null)
            {
                continue;
            }

            if (_collidersRegistry.TryGetEntity(collider, out Entity target) == false)
            {
                continue;
            }

            if (_targets.Value.Contains(target))
            {
                continue;
            }

            if (CombatTargetFilter.CanDamage(_entity, target) == false)
            {
                continue;
            }

            _targets.Value.Add(target);
        }

        _targetsCollected.Invoke(position);
    }
}