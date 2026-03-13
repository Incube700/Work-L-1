using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class MineTriggerExplosionSystem : IInitializableSystem, IUpdatableSystem
{
    private const int BufferSize = 32;

    private readonly ExplosionService _explosionService;
    private readonly CollidersRegistryService _colliders;
    private readonly float _triggerRadius;
    private readonly float _explosionRadius;
    private readonly float _damage;
    private readonly int _mask;
    private readonly Collider[] _buffer = new Collider[BufferSize];

    private Entity _entity;
    private Transform _transform;
    private ReactiveVariable<bool> _isDead;
    private bool _triggered;

    public MineTriggerExplosionSystem(
        ExplosionService explosionService,
        CollidersRegistryService colliders,
        float triggerRadius,
        float explosionRadius,
        float damage,
        int mask)
    {
        _explosionService = explosionService;
        _colliders = colliders;
        _triggerRadius = triggerRadius;
        _explosionRadius = explosionRadius;
        _damage = damage;
        _mask = mask;
    }

    public void OnInit(Entity entity)
    {
        _entity = entity;
        _transform = entity.Transform;
        _isDead = entity.IsDead;
        _triggered = false;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_triggered || _transform == null || _isDead.Value)
        {
            return;
        }

        int count = Physics.OverlapSphereNonAlloc(
            _transform.position,
            _triggerRadius,
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

            if (_colliders.TryGetEntity(collider, out Entity target) == false)
            {
                continue;
            }

            if (target.HasComponent<MoveDirection>() == false)
            {
                continue;
            }

            Log($"[Defend] Mine triggered at {_transform.position}");
            _explosionService.Explode(_transform.position, _explosionRadius, _damage, _mask);

            if (_entity.HasComponent<TakeDamageRequest>())
            {
                _entity.TakeDamageRequest.Invoke(_entity.CurrentHealth.Value);
            }

            _triggered = true;
            break;
        }
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        Debug.Log(message);
    }
}