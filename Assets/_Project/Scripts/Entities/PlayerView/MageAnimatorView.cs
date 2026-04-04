using UnityEngine;

public sealed class MageAnimatorView : MonoBehaviour
{
    private static readonly int AttackKey = Animator.StringToHash("Attack");
    private static readonly int DieKey = Animator.StringToHash("Die");

    [SerializeField] private Animator _animator;

    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    public void PlayAttack()
    {
        if (_animator == null)
        {
            return;
        }

        _animator.SetTrigger(AttackKey);
    }

    public void PlayDie()
    {
        if (_animator == null)
        {
            return;
        }

        _animator.SetTrigger(DieKey);
    }
}