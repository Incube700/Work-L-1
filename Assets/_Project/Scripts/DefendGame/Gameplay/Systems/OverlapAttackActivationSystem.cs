using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class OverlapAttackActivationSystem : IInitializableSystem, IUpdatableSystem
{
    private const int BufferSize = 32;

    private readonly CollidersRegistryService _collidersRegistry;
    private readonly Collider[] _buffer = new Collider[BufferSize];

    private Entity _entity;
    private Transform _transform;
    private float _radius;
    private int _mask;
    private SimpleEvent<Vector3> _attackRequest;
    private ReactiveVariable<bool> _isDead;

    public OverlapAttackActivationSystem(CollidersRegistryService collidersRegistry)
    {
        _collidersRegistry = collidersRegistry;
    }

    public void OnInit(Entity entity)
    {
        _entity = entity;
        _transform = entity.Transform;
        _radius = entity.GetComponent<OverlapActivationRadius>().Value;
        _mask = entity.GetComponent<OverlapActivationMask>().Value;
        _attackRequest = entity.GetComponent<AreaAttackRequest>().Value;
        _isDead = entity.IsDead;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_isDead.Value)
        {
            return;
        }

        int count = Physics.OverlapSphereNonAlloc(
            _transform.position,
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

            if (CombatTargetFilter.CanDamage(_entity, target) == false)
            {
                continue;
            }

            _attackRequest.Invoke(_transform.position);
            return;
        }
    }
}