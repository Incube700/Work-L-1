using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class TimedAreaAttackRequestSystem : IInitializableSystem, IUpdatableSystem
{
    private Transform _transform;
    private float _interval;
    private ReactiveVariable<float> _timeLeft;
    private SimpleEvent<Vector3> _attackRequest;
    private ReactiveVariable<bool> _isDead;

    public void OnInit(Entity entity)
    {
        _transform = entity.Transform;
        _interval = entity.GetComponent<TimedAreaAttackInterval>().Value;
        _timeLeft = entity.GetComponent<TimedAreaAttackLeft>().Value;
        _attackRequest = entity.GetComponent<AreaAttackRequest>().Value;
        _isDead = entity.IsDead;

        _timeLeft.Value = _interval;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_isDead.Value)
        {
            return;
        }

        _timeLeft.Value -= deltaTime;

        if (_timeLeft.Value > 0f)
        {
            return;
        }

        _timeLeft.Value = _interval;
        _attackRequest.Invoke(_transform.position);
    }
}