using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class TurretProjectileView : MonoBehaviour
{
    private Entity _target;
    private ExplosionService _explosionService;
    private float _damage;
    private float _impactRadius;
    private float _speed;
    private float _lifeTime;
    private float _hitDistance;
    private float _timeLeft;
    private bool _isInitialized;

    public void Initialize(
        Entity target,
        ExplosionService explosionService,
        float damage,
        float impactRadius,
        float speed,
        float lifeTime,
        float hitDistance)
    {
        _target = target;
        _explosionService = explosionService;
        _damage = damage;
        _impactRadius = impactRadius;
        _speed = speed;
        _lifeTime = lifeTime;
        _hitDistance = hitDistance;
        _timeLeft = lifeTime;
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
            Destroy(gameObject);
            return;
        }

        Vector3 currentPoint = transform.position;
        currentPoint.y = 0f;

        targetPoint.y = 0f;

        if (Vector3.Distance(currentPoint, targetPoint) <= _hitDistance)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (_target != null && _target.IsDead.Value == false)
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