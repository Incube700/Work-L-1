using UnityEngine;
using Assets._Project.Scripts.Gameplay.EntitiesCore;

public sealed class MageDeathAnimationView : EntityView
{
    [SerializeField] private MageAnimatorView _animatorView;

    protected override void Awake()
    {
        base.Awake();

        if (_animatorView == null)
        {
            _animatorView = GetComponent<MageAnimatorView>();
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

        if (_animatorView == null)
        {
            return;
        }

        _animatorView.PlayDie();
    }
}