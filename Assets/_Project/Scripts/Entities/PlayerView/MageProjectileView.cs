using UnityEngine;

public sealed class MageProjectileView : MonoBehaviour
{
    [SerializeField] private float _speed = 14f;
    [SerializeField] private float _lifeTime = 3f;
    [SerializeField] private float _hitDistance = 0.2f;

    private ExplosionService _explosionService;
    private Vector3 _targetPoint;
    private Vector3 _direction;
    private float _radius;
    private float _damage;
    private int _mask;
    private float _timeLeft;
    private bool _isInitialized;

    public void Initialize(
        Vector3 targetPoint,
        ExplosionService explosionService,
        float radius,
        float damage,
        int mask)
    {
        _targetPoint = targetPoint;
        _explosionService = explosionService;
        _radius = radius;
        _damage = damage;
        _mask = mask;
        _timeLeft = _lifeTime;

        Vector3 direction = _targetPoint - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            _direction = transform.forward;
        }
        else
        {
            _direction = direction.normalized;
            transform.rotation = Quaternion.LookRotation(_direction);
        }

        _isInitialized = true;
    }

    private void Update()
    {
        if (_isInitialized == false)
        {
            return;
        }

        transform.position += _direction * (_speed * Time.deltaTime);

        _timeLeft -= Time.deltaTime;

        if (_timeLeft <= 0f)
        {
            Explode();
            return;
        }

        Vector3 currentPoint = transform.position;
        currentPoint.y = 0f;

        Vector3 targetPoint = _targetPoint;
        targetPoint.y = 0f;

        if (Vector3.Distance(currentPoint, targetPoint) <= _hitDistance)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (_explosionService != null)
        {
            _explosionService.Explode(_targetPoint, _radius, _damage, _mask);
        }

        Destroy(gameObject);
    }
}