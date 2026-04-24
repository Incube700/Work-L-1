using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using UnityEngine;

public sealed class TurretAttackService
{
    private const float SpawnForwardOffset = 0.6f;
    private const float SpawnHeightOffset = 0.35f;

    private readonly ExplosionService _explosionService;
    private readonly float _attackInterval;
    private readonly float _damage;
    private readonly float _impactRadius;
    private readonly float _projectileSpeed;
    private readonly float _projectileLifeTime;
    private readonly float _projectileHitDistance;
    private readonly GameObject _projectilePrefab;

    private float _attackLeft;

    public TurretAttackService(
        ExplosionService explosionService,
        float attackInterval,
        float damage,
        float impactRadius,
        string projectilePrefabPath,
        float projectileSpeed,
        float projectileLifeTime,
        float projectileHitDistance)
    {
        _explosionService = explosionService ?? throw new ArgumentNullException(nameof(explosionService));
        _attackInterval = attackInterval;
        _damage = damage;
        _impactRadius = impactRadius;
        _projectileSpeed = projectileSpeed;
        _projectileLifeTime = projectileLifeTime;
        _projectileHitDistance = projectileHitDistance;

        if (string.IsNullOrWhiteSpace(projectilePrefabPath))
        {
            Debug.LogError("Turret projectile prefab path is empty.");
        }
        else
        {
            _projectilePrefab = Resources.Load<GameObject>(projectilePrefabPath);

            if (_projectilePrefab == null)
            {
                Debug.LogError($"Turret projectile prefab not found: {projectilePrefabPath}");
            }
        }

        _attackLeft = 0f;
    }

    public void Tick(float deltaTime)
    {
        if (_attackLeft > 0f)
        {
            _attackLeft -= deltaTime;
        }
    }

    public void TryAttack(Transform shooterTransform, Entity target, Vector3 direction)
    {
        if (_attackLeft > 0f)
        {
            return;
        }

        if (shooterTransform == null)
        {
            return;
        }

        if (target == null || target.IsDead.Value)
        {
            return;
        }

        if (_projectilePrefab == null)
        {
            return;
        }

        Vector3 spawnDirection = direction;
        spawnDirection.y = 0f;

        if (spawnDirection.sqrMagnitude <= 0.001f)
        {
            spawnDirection = shooterTransform.forward;
        }
        else
        {
            spawnDirection.Normalize();
        }

        Vector3 spawnPosition =
            shooterTransform.position +
            spawnDirection * SpawnForwardOffset +
            Vector3.up * SpawnHeightOffset;

        Quaternion rotation = Quaternion.LookRotation(spawnDirection);

        GameObject instance = UnityEngine.Object.Instantiate(
            _projectilePrefab,
            spawnPosition,
            rotation);

        TurretProjectileView projectileView = instance.GetComponent<TurretProjectileView>();

        if (projectileView == null)
        {
            Debug.LogError("TurretProjectileView is missing on turret projectile prefab.");
            UnityEngine.Object.Destroy(instance);
            return;
        }

        projectileView.Initialize(
            target,
            _explosionService,
            _damage,
            _impactRadius,
            _projectileSpeed,
            _projectileLifeTime,
            _projectileHitDistance);

        _attackLeft = _attackInterval;
    }
}