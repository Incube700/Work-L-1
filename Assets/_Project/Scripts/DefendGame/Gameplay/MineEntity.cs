using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

public sealed class MineEntity
{
    private const int BufferSize = 32;

    private readonly EntitiesLifeContext _life;
    private readonly ExplosionService _explosionService;
    private readonly Collider[] _buffer = new Collider[BufferSize];
    private readonly CollidersRegistryService _colliders;

    private Entity _entity;
    private Transform _transform;
    private readonly float _triggerRadius;
    private readonly float _explosionRadius;
    private readonly float _damage;
    private readonly int _mask;

    public MineEntity(
        EntitiesLifeContext life,
        ExplosionService explosionService,
        CollidersRegistryService colliders,
        Entity entity,
        Transform transform,
        float triggerRadius,
        float explosionRadius,
        float damage,
        int mask)
    {
        _life = life;
        _explosionService = explosionService;
        _colliders = colliders;
        _entity = entity;
        _transform = transform;
        _triggerRadius = triggerRadius;
        _explosionRadius = explosionRadius;
        _damage = damage;
        _mask = mask;
    }

    public bool IsReleased { get; private set; }

    public void Update(float deltaTime)
    {
        if (IsReleased || _transform == null)
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
            Release();
            break;
        }
    }

    public void Release()
    {
        if (IsReleased)
        {
            return;
        }

        IsReleased = true;

        if (_entity != null)
        {
            _life.Release(_entity);
        }

        _entity = null;
        _transform = null;
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        Debug.Log(message);
    }
}
