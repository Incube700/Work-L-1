using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;

public sealed class AreaAttackSelfReleaseSystem : IInitializableSystem, IDisposableSystem
{
    private SimpleEvent _attackFinished;
    private ReactiveVariable<bool> _isDead;
    private IDisposable _attackFinishedSubscription;

    public void OnInit(Entity entity)
    {
        _attackFinished = entity.GetComponent<AreaAttackFinished>().Value;
        _isDead = entity.IsDead;

        _attackFinishedSubscription = _attackFinished.Subscribe(OnAttackFinished);
    }

    public void OnDispose()
    {
        _attackFinishedSubscription?.Dispose();
        _attackFinishedSubscription = null;
    }

    private void OnAttackFinished()
    {
        _isDead.Value = true;
    }
}