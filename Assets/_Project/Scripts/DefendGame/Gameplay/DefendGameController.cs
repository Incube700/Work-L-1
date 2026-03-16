using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using UnityEngine;

public sealed class DefendGameController : IDisposable
{
    private readonly EntitiesLifeContext _life;
    private readonly DefendPhaseService _phaseService;
    private readonly WaveProgressService _waveProgressService;
    private readonly BuildingStateService _buildingStateService;
    private readonly DefendResultService _resultService;
    private readonly EnemyService _enemyService;
    private readonly DefendInputHandler _inputHandler;
    private readonly DefendStateMachine _stateMachine;

    public DefendGameController(
        EntitiesLifeContext life,
        DefendPhaseService phaseService,
        WaveProgressService waveProgressService,
        BuildingStateService buildingStateService,
        DefendResultService resultService,
        EnemyService enemyService,
        DefendInputHandler inputHandler,
        DefendStateMachine stateMachine)
    {
        _life = life;
        _phaseService = phaseService;
        _waveProgressService = waveProgressService;
        _buildingStateService = buildingStateService;
        _resultService = resultService;
        _enemyService = enemyService;
        _inputHandler = inputHandler;
        _stateMachine = stateMachine;
    }

    public DefendPhase Phase => _phaseService.CurrentPhase;
    public int AliveEnemiesCount => _enemyService.AliveCount;
    public int WavesCount => _waveProgressService.WavesCount;
    public int CurrentWaveNumber => _waveProgressService.CurrentWaveNumber;
    public float BuildingCurrentHealth => _buildingStateService.CurrentHealth;
    public float BuildingMaxHealth => _buildingStateService.MaxHealth;

    public void Start()
    {
        if (_buildingStateService.HasBuilding == false)
        {
            throw new InvalidOperationException("Building is not initialized.");
        }

        _life.Released += OnEntityReleased;

        Log($"[Defend] Session started. Building HP: {_buildingStateService.MaxHealth}");

        if (_waveProgressService.HasAnyWaves == false)
        {
            _resultService.Win();
            return;
        }

        _stateMachine.Enter();
    }

    public void Update(float deltaTime)
    {
        if (_phaseService.IsEnded)
        {
            return;
        }

        _stateMachine.Update(deltaTime);

        float buildingY = _buildingStateService.HasBuilding
            ? _buildingStateService.Building.Transform.position.y
            : 0f;

        _inputHandler.Update(_phaseService.CurrentPhase, buildingY);
    }

    public void Dispose()
    {
        _life.Released -= OnEntityReleased;

        _stateMachine?.Dispose();
        _enemyService.Dispose();
    }

    private void OnEntityReleased(Entity entity)
    {
        _enemyService.Remove(entity);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        Debug.Log(message);
    }
}