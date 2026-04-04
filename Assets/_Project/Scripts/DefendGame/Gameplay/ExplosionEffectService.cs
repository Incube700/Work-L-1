using System;
using UnityEngine;

public sealed class ExplosionEffectService : IDisposable
{
    private const float DefaultDestroyDelay = 5f;

    private readonly ExplosionService _explosionService;
    private readonly GameObject _effectPrefab;
    private readonly float _heightOffset;
    private readonly float _radiusScaleMultiplier;

    public ExplosionEffectService(
        ExplosionService explosionService,
        GameObject effectPrefab,
        float heightOffset = 0.3f,
        float radiusScaleMultiplier = 0.5f)
    {
        _explosionService = explosionService ?? throw new ArgumentNullException(nameof(explosionService));
        _effectPrefab = effectPrefab;
        _heightOffset = heightOffset;
        _radiusScaleMultiplier = radiusScaleMultiplier;
    }

    public void Initialize()
    {
        _explosionService.Exploded += OnExploded;
    }

    public void Dispose()
    {
        _explosionService.Exploded -= OnExploded;
    }

    private void OnExploded(Vector3 position, float radius)
    {
        if (_effectPrefab == null)
        {
            return;
        }

        Vector3 effectPosition = position + Vector3.up * _heightOffset;

        GameObject instance = UnityEngine.Object.Instantiate(
            _effectPrefab,
            effectPosition,
            Quaternion.identity);

        float scale = Mathf.Max(0.1f, radius * _radiusScaleMultiplier);
        instance.transform.localScale = Vector3.one * scale;

        ConfigureParticleSystems(instance);
    }

    private void ConfigureParticleSystems(GameObject instance)
    {
        ParticleSystem[] particleSystems = instance.GetComponentsInChildren<ParticleSystem>(true);

        for (int i = 0; i < particleSystems.Length; i++)
        {
            ParticleSystem.MainModule main = particleSystems[i].main;
            main.stopAction = ParticleSystemStopAction.Destroy;
        }
    }
}