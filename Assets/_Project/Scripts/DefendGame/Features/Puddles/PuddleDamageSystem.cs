using System.Collections.Generic;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using UnityEngine;

public sealed class PuddleDamageSystem : IInitializableSystem, IUpdatableSystem
{
    private const int BufferSize = 32;

    private readonly PuddleTargetCollectorService _targetCollector;
    private readonly PuddleDamageService _damageService;
    private readonly float _tickInterval;
    private readonly List<Entity> _targets = new List<Entity>(BufferSize);

    private Transform _transform;
    private float _tickLeft;

    public PuddleDamageSystem(
        PuddleTargetCollectorService targetCollector,
        PuddleDamageService damageService,
        float tickInterval)
    {
        _targetCollector = targetCollector;
        _damageService = damageService;
        _tickInterval = tickInterval;
    }

    public void OnInit(Entity entity)
    {
        _transform = entity.Transform;
        _tickLeft = _tickInterval;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_transform == null)
        {
            return;
        }

        _tickLeft -= deltaTime;

        if (_tickLeft > 0f)
        {
            return;
        }

        _tickLeft = _tickInterval;

        _targetCollector.Collect(_transform.position, _targets);

        int damagedCount = _damageService.Apply(_targets);

        if (damagedCount > 0)
        {
            Log($"[Defend] Puddle tick. Damaged: {damagedCount}");
        }
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        Debug.Log(message);
    }
}