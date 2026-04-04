using UnityEngine;

public sealed class MageAttackAnimationView : MonoBehaviour
{
    [SerializeField] private MageAnimatorView _mageAnimatorView;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private MageProjectileView _projectilePrefab;

    private DefendInputHandler _inputHandler;
    private DefendLevelConfig _level;
    private ExplosionService _explosionService;
    private bool _isSubscribed;

    public void Construct(
        DefendInputHandler inputHandler,
        DefendLevelConfig level,
        ExplosionService explosionService)
    {
        Unsubscribe();

        _inputHandler = inputHandler;
        _level = level;
        _explosionService = explosionService;

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

        if (_inputHandler == null)
        {
            return;
        }

        _inputHandler.PlayerAttacked += OnPlayerAttacked;
        _isSubscribed = true;
    }

    private void Unsubscribe()
    {
        if (_isSubscribed == false)
        {
            return;
        }

        if (_inputHandler != null)
        {
            _inputHandler.PlayerAttacked -= OnPlayerAttacked;
        }

        _isSubscribed = false;
    }

    private void OnPlayerAttacked(Vector3 targetPoint)
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
            _level.PlayerExplosionConfig.Damage,
            _level.PlayerExplosionConfig.Mask);
    }
}