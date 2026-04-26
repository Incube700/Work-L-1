using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class ProjectileShootSystem : IInitializableSystem, IUpdatableSystem, IDisposableSystem
{
    private const float SpawnForwardOffset = 0.6f;
    private const float SpawnHeightOffset = 0.35f;

    private readonly ProjectileFactory _projectileFactory;

    private Transform _transform;
    private Team _team;
    private ProjectileConfig _projectileConfig;
    private ReactiveVariable<float> _cooldown;
    private ReactiveVariable<bool> _isDead;
    private float _interval;
    private SimpleEvent<Vector3> _shootRequest;
    private IDisposable _shootRequestSubscription;

    public ProjectileShootSystem(ProjectileFactory projectileFactory)
    {
        _projectileFactory = projectileFactory ?? throw new ArgumentNullException(nameof(projectileFactory));
    }

    public void OnInit(Entity entity)
    {
        _transform = entity.Transform;
        _team = entity.GetComponent<TeamComponent>().Value;
        _projectileConfig = entity.GetComponent<ProjectileShootConfig>().Value;
        _interval = entity.GetComponent<ProjectileShootInterval>().Value;
        _cooldown = entity.GetComponent<ProjectileShootCooldown>().Value;
        _shootRequest = entity.GetComponent<ProjectileShootRequest>().Value;

        if (entity.TryGetComponent(out IsDead isDead))
        {
            _isDead = isDead.Value;
        }

        _shootRequestSubscription = _shootRequest.Subscribe(OnShootRequested);
    }

    public void OnUpdate(float deltaTime)
    {
        if (IsDead())
        {
            return;
        }

        if (_cooldown.Value <= 0f)
        {
            return;
        }

        _cooldown.Value -= deltaTime;
    }

    public void OnDispose()
    {
        _shootRequestSubscription?.Dispose();
        _shootRequestSubscription = null;
    }

    private void OnShootRequested(Vector3 targetPoint)
    {
        if (IsDead())
        {
            return;
        }

        if (_cooldown.Value > 0f)
        {
            return;
        }

        if (_projectileConfig == null)
        {
            Debug.LogError("Projectile shoot config is not assigned.");
            return;
        }

        Vector3 direction = targetPoint - _transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            return;
        }

        Vector3 normalizedDirection = direction.normalized;

        Vector3 spawnPosition =
            _transform.position +
            normalizedDirection * SpawnForwardOffset +
            Vector3.up * SpawnHeightOffset;

        _projectileFactory.Create(
            spawnPosition,
            normalizedDirection,
            targetPoint,
            _team,
            _projectileConfig);

        _cooldown.Value = _interval;
    }

    private bool IsDead()
    {
        return _isDead != null && _isDead.Value;
    }
}