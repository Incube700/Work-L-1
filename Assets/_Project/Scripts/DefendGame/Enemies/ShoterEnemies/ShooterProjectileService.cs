using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Infrastructure.AssetsManagement;
using UnityEngine;

public sealed class ShooterProjectileService
{
    private const float SpawnForwardOffset = 0.6f;
    private const float SpawnHeightOffset = 0.35f;

    private readonly ResourcesAssetsLoader _assetsLoader;
    private readonly ExplosionService _explosionService;

    public ShooterProjectileService(
        ResourcesAssetsLoader assetsLoader,
        ExplosionService explosionService)
    {
        _assetsLoader = assetsLoader ?? throw new ArgumentNullException(nameof(assetsLoader));
        _explosionService = explosionService ?? throw new ArgumentNullException(nameof(explosionService));
    }

    public void Spawn(
        Transform shooterTransform,
        Vector3 direction,
        Entity target,
        string projectilePrefabPath,
        float damage,
        float impactRadius)
    {
        if (shooterTransform == null)
        {
            return;
        }

        if (target == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(projectilePrefabPath))
        {
            Debug.LogError("Shooter projectile prefab path is empty.");
            return;
        }

        GameObject projectilePrefab = _assetsLoader.Load<GameObject>(projectilePrefabPath);

        if (projectilePrefab == null)
        {
            Debug.LogError($"Shooter projectile prefab not found: {projectilePrefabPath}");
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
            projectilePrefab,
            spawnPosition,
            rotation);

        ShooterProjectileView projectileView = instance.GetComponent<ShooterProjectileView>();

        if (projectileView == null)
        {
            Debug.LogError("ShooterProjectileView is missing on projectile prefab.");
            UnityEngine.Object.Destroy(instance);
            return;
        }

        projectileView.Initialize(
            target,
            _explosionService,
            impactRadius,
            damage);
    }
}