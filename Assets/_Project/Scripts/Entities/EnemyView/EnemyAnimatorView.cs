using UnityEngine;
using Assets._Project.Scripts.Gameplay.EntitiesCore;

public sealed class EnemyAnimatorView : EntityView
{
    private static readonly int DieKey = Animator.StringToHash("Die");

    [SerializeField] private Animator _animator;

    protected override void Awake()
    {
        base.Awake();

        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    protected override void OnBind(Entity entity)
    {
        entity.IsDead.Changed += OnIsDeadChanged;
    }

    protected override void OnUnbind(Entity entity)
    {
        entity.IsDead.Changed -= OnIsDeadChanged;
    }

    private void OnIsDeadChanged()
    {
        if (LinkedEntity.IsDead.Value == false)
        {
            return;
        }

        if (_animator == null)
        {
            return;
        }

        _animator.SetTrigger(DieKey);
    }
}