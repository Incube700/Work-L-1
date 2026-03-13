using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;
using UnityEngine;

public enum DefendPhase
{
    Wave,
    Rest,
    Ended
}

public sealed class DefendGameController : IDisposable
{
    private readonly DefendLevelConfig _level;
    private readonly DefendEntitiesFactory _factory;
    private readonly EntitiesLifeContext _life;
    private readonly PlayerProgressService _progress;
    private readonly GameFlowService _flow;
    private readonly DefendInputHandler _inputHandler;
    private readonly EnemyService _enemyService;

    private Entity _building;
    private int _currentWaveIndex = -1;
    private DefendPhase _phase;
    private DefendStateMachine _stateMachine;
    private EnemySpawner _enemySpawner;

    public DefendGameController(
        DefendLevelConfig level,
        DefendEntitiesFactory factory,
        EntitiesLifeContext life,
        IInputService input,
        IPointerService pointer,
        ExplosionService explosions,
        CollidersRegistryService colliders,
        WalletService wallet,
        PlayerProgressService progress,
        GameFlowService flow)
    {
        _level = level;
        _factory = factory;
        _life = life;
        _progress = progress;
        _flow = flow;

        MineFactory mineFactory = new MineFactory(
            _level,
            _factory,
            _life,
            explosions,
            colliders);

        MinePlacementService minePlacementService = new MinePlacementService(
            _level,
            wallet,
            mineFactory);

        _inputHandler = new DefendInputHandler(
            input,
            pointer,
            explosions,
            minePlacementService,
            _level);

        _enemyService = new EnemyService();
    }

    public DefendPhase Phase => _phase;
    public int AliveEnemiesCount => _enemyService.AliveCount;
    public int WavesCount => _level.Waves.Count;
    public int CurrentWaveNumber => _currentWaveIndex + 1;
    public float RestDurationSeconds => _level.RestDurationSeconds;
    public bool HasNextWave => _currentWaveIndex + 1 < _level.Waves.Count;
    public bool IsBuildingDead => _building != null && _building.IsDead.Value;
    public bool IsWaveSpawnCompleted => _enemySpawner != null && _enemySpawner.IsCompleted;

    public void Start(Vector3 buildingPosition)
    {
        if (_level.WinRewardGold <= 0)
        {
            throw new InvalidOperationException("DefendLevelConfig.WinRewardGold must be > 0.");
        }

        _building = _factory.CreateBuilding(buildingPosition, _level);
        _life.Released += OnEntityReleased;

        _enemySpawner = new EnemySpawner(
            _level,
            _factory,
            _building,
            OnEnemySpawned);

        Log($"[Defend] Session started. Building HP: {_level.BuildingHealth}");

        if (_level.Waves == null || _level.Waves.Count == 0)
        {
            Win();
            return;
        }

        DefendStateMachineFactory stateMachineFactory = new DefendStateMachineFactory();
        _stateMachine = stateMachineFactory.Create(this);
        _stateMachine.Enter();
    }

    public void Update(float deltaTime)
    {
        if (_phase == DefendPhase.Ended)
        {
            return;
        }

        _stateMachine.Update(deltaTime);

        float buildingY = _building != null
            ? _building.Transform.position.y
            : 0f;

        _inputHandler.Update(_phase, buildingY);
    }

    public void Dispose()
    {
        _life.Released -= OnEntityReleased;

        _stateMachine?.Dispose();
        _stateMachine = null;
        _enemySpawner = null;

        _enemyService.Dispose();
    }

    public void SetPhase(DefendPhase phase)
    {
        _phase = phase;
    }

    public int MoveToNextWave()
    {
        _currentWaveIndex++;
        return _currentWaveIndex;
    }

    public DefendLevelConfig.WaveConfig GetWaveConfig(int waveIndex)
    {
        return _level.Waves[waveIndex];
    }

    public void StartWaveSpawn(DefendLevelConfig.WaveConfig wave)
    {
        _enemySpawner.StartWave(wave);
    }

    public void UpdateWaveSpawn(float deltaTime)
    {
        _enemySpawner.Update(deltaTime);
    }

    public void Win()
    {
        _phase = DefendPhase.Ended;
        _progress.RegisterWin(_level.WinRewardGold);
        Log($"[Defend] WIN. Reward: {_level.WinRewardGold} gold.");
        _flow.OpenMainMenu();
    }

    public void Lose()
    {
        _phase = DefendPhase.Ended;
        _progress.RegisterLoss();
        Log("[Defend] LOSE.");
        _flow.OpenMainMenu();
    }

    public void LogState(string message)
    {
        Log(message);
    }

    private void OnEnemySpawned(Entity enemy)
    {
        _enemyService.Add(enemy);
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