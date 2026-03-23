using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class EnemyAnimatorView : MonoBehaviour
{
    private static readonly int DieKey = Animator.StringToHash("Die");

    [SerializeField] private MonoEntity _monoEntity;
    [SerializeField] private Animator _animator;

    private ReactiveVariable<bool> _isDead;
    private bool _isBound;
    private bool _deathPlayed;

    private void Awake()
    {
        if (_monoEntity == null)
        {
            _monoEntity = GetComponentInParent<MonoEntity>();
        }

        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        if (_isBound == false)
        {
            TryBind();
        }
    }

    private void OnDestroy()
    {
        Unbind();
    }

    private void TryBind()
    {
        if (_monoEntity == null || _monoEntity.LinkedEntity == null)
        {
            return;
        }

        Entity entity = _monoEntity.LinkedEntity;

        if (entity.TryGetComponent<IsDead>(out var isDeadComponent) == false)
        {
            return;
        }

        _isDead = isDeadComponent.Value;
        _isDead.Changed += OnIsDeadChanged;
        _isBound = true;
    }

    private void Unbind()
    {
        if (_isDead != null)
        {
            _isDead.Changed -= OnIsDeadChanged;
            _isDead = null;
        }

        _isBound = false;
    }

    private void OnIsDeadChanged()
    {
        if (_isDead.Value == false || _deathPlayed)
        {
            return;
        }

        _deathPlayed = true;

        if (_animator != null)
        {
            _animator.SetTrigger(DieKey);
        }
    }
}