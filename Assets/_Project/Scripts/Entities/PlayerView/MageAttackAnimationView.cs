using UnityEngine;

public sealed class MageAttackAnimationView : MonoBehaviour
{
    [SerializeField] private MageAnimatorView _mageAnimatorView;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private MageProjectileView _projectilePrefab;

    private BuildingCombatService _buildingCombatService;
    private DefendLevelConfig _level;
    private ExplosionService _explosionService;
    private float _damageMultiplier = 1f;
    private bool _isSubscribed;

    public void Construct(
        BuildingCombatService buildingCombatService,
        DefendLevelConfig level,
        ExplosionService explosionService,
        float damageMultiplier)
    {
        Unsubscribe();

        _buildingCombatService = buildingCombatService;
        _level = level;
        _explosionService = explosionService;
        _damageMultiplier = damageMultiplier > 0f ? damageMultiplier : 1f;

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

        SpawnProjectile(targetPoint);
    }

    private void SpawnProjectile(Vector3 targetPoint)
    {
        if (_projectilePrefab == null)
        {
            return;
        }

        if (_projectileSpawnPoint == null)
        {
            return;
        }

        if (_level == null)
        {
            return;
        }

        if (_explosionService == null)
        {
            return;
        }

        MageProjectileView projectile = Instantiate(
            _projectilePrefab,
            _projectileSpawnPoint.position,
            Quaternion.identity);

        projectile.Initialize(
            targetPoint,
            _explosionService,
            _level.PlayerExplosionConfig.Radius,
            _level.PlayerExplosionConfig.Damage * _damageMultiplier,
            _level.PlayerExplosionConfig.Mask);
    }
}
