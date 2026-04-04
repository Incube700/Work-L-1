using UnityEngine;

public sealed class MageClickAttackView : MonoBehaviour
{
    [SerializeField] private MageAnimatorView _mageAnimatorView;

    private void Awake()
    {
        if (_mageAnimatorView == null)
        {
            _mageAnimatorView = GetComponent<MageAnimatorView>();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) == false)
        {
            return;
        }

        _mageAnimatorView?.PlayAttack();
    }
}