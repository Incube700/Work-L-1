using UnityEngine;

public sealed class MageAttackAnimationView : MonoBehaviour
{
    [SerializeField] private MageAnimatorView _mageAnimatorView;
    [SerializeField] private Transform _projectileSpawnPoint;

    private BuildingCombatService _buildingCombatService;
    private bool _isSubscribed;

    public Transform ProjectileSpawnPoint => _projectileSpawnPoint != null
        ? _projectileSpawnPoint
        : transform;

    public void Construct(BuildingCombatService buildingCombatService)
    {
        Unsubscribe();

        _buildingCombatService = buildingCombatService;

        Subscribe();
    }

    private void Awake()
    {
        if (_mageAnimatorView == null)
        {
            _mageAnimatorView = GetComponent<MageAnimatorView>();
        }

        if (_projectileSpawnPoint == null)
        {
            _projectileSpawnPoint = transform;
        }
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (_isSubscribed)
        {
            return;
        }

        if (_buildingCombatService == null)
        {
            return;
        }

        _buildingCombatService.AttackPerformed += OnAttackPerformed;
        _isSubscribed = true;
    }

    private void Unsubscribe()
    {
        if (_isSubscribed == false)
        {
            return;
        }

        if (_buildingCombatService != null)
        {
            _buildingCombatService.AttackPerformed -= OnAttackPerformed;
        }

        _isSubscribed = false;
    }

    private void OnAttackPerformed(Vector3 targetPoint)
    {
        if (_mageAnimatorView != null)
        {
            _mageAnimatorView.PlayAttack();
        }
    }
}