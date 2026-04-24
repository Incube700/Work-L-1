using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class ShooterProjectileView : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifeTime = 3f;
    [SerializeField] private float _hitDistance = 0.2f;

    private Entity _target;
    private ExplosionService _explosionService;
    private float _impactRadius;
    private float _damage;
    private float _timeLeft;
    private bool _isInitialized;

    public void Initialize(
        Entity target,
        ExplosionService explosionService,
        float impactRadius,
        float damage)
    {
        _target = target;
        _explosionService = explosionService;
        _impactRadius = impactRadius;
        _damage = damage;
        _timeLeft = _lifeTime;
        _isInitialized = true;
    }

    private void Update()
    {
        if (_isInitialized == false)
        {
            return;
        }

        if (_target == null || _target.IsDead.Value)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPoint = _target.Transform.position;
        Vector3 direction = targetPoint - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Vector3 normalizedDirection = direction.normalized;
            transform.position += normalizedDirection * (_speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(normalizedDirection);
        }

        _timeLeft -= Time.deltaTime;

        if (_timeLeft <= 0f)
        {
            Explode(false);
            return;
        }

        Vector3 currentPoint = transform.position;
        currentPoint.y = 0f;

        targetPoint.y = 0f;

        if (Vector3.Distance(currentPoint, targetPoint) <= _hitDistance)
        {
            Explode(true);
        }
    }

    private void Explode(bool applyDamage)
    {
        if (applyDamage && _target != null && _target.IsDead.Value == false)
        {
            if (_target.HasComponent<TakeDamageRequest>())
            {
                _target.TakeDamageRequest.Invoke(_damage);
            }
        }

        if (_explosionService != null)
        {
            _explosionService.NotifyExploded(transform.position, _impactRadius);
        }

        Destroy(gameObject);
    }
}