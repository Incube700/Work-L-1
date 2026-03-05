using System;
using System.Collections.Generic;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.AIFeature;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;
using Assets._Project.Scripts.Utilities.Conditions;
using Assets._Project.Scripts.Utilities.StateMachineCore;
using Assets._Project.Scripts.Utilities.Timers;
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
    private readonly IInputService _input;
    private readonly IPointerService _pointer;
    private readonly ExplosionService _explosions;
    private readonly CollidersRegistryService _colliders;
    private readonly WalletService _wallet;
    private readonly PlayerProgressService _progress;
    private readonly GameFlowService _flow;

    private readonly List<Entity> _aliveEnemies = new List<Entity>();
    private readonly List<MineEntity> _mines = new List<MineEntity>();

    private Entity _building;
    private int _currentWaveIndex = -1;
    private DefendPhase _phase;
    private CooldownTimer _restTimer;
    private AIStateMachine _stateMachine;
    private int _waveEnemiesToSpawn;
    private int _waveEnemiesSpawned;
    private float _waveSpawnInterval;
    private float _waveSpawnTimer;
    private bool _waveSpawnCompleted;
    private bool _waveSpawnCompletedLogged;
    private bool _waveClearedLogged;

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
        _input = input;
        _pointer = pointer;
        _explosions = explosions;
        _colliders = colliders;
        _wallet = wallet;
        _progress = progress;
        _flow = flow;
    }

    public DefendPhase Phase => _phase;

    public void Start(Vector3 buildingPosition)
    {
        if (_level.WinRewardGold <= 0)
        {
            throw new InvalidOperationException("DefendLevelConfig.WinRewardGold must be > 0.");
        }

        _building = _factory.CreateBuilding(buildingPosition, _level);
        _life.Released += OnEntityReleased;
        Log($"[Defend] Session started. Building HP: {_level.BuildingHealth}");

        if (_level.Waves == null || _level.Waves.Count == 0)
        {
            OnWinEntered();
            return;
        }

        BuildStateMachine();
        _stateMachine.Enter();
    }

    public void Update(float deltaTime)
    {
        if (_phase == DefendPhase.Ended)
        {
            return;
        }

        _stateMachine.Update(deltaTime);
        ProcessInput();
        UpdateMines(deltaTime);
    }

    public void Dispose()
    {
        _life.Released -= OnEntityReleased;

        _stateMachine?.Dispose();
        _stateMachine = null;

        for (int i = 0; i < _mines.Count; i++)
        {
            _mines[i].Release();
        }

        _mines.Clear();
        _aliveEnemies.Clear();
    }

    private void BuildStateMachine()
    {
        _stateMachine = new AIStateMachine();

        EmptyState waveState = new EmptyState(OnWaveStateUpdated);
        EmptyState restState = new EmptyState(OnRestStateUpdated);
        EmptyState winState = new EmptyState();
        EmptyState loseState = new EmptyState();

        waveState.Entered.Invoked += OnWaveEntered;
        restState.Entered.Invoked += OnRestEntered;
        winState.Entered.Invoked += OnWinEntered;
        loseState.Entered.Invoked += OnLoseEntered;

        _stateMachine.AddState(waveState);
        _stateMachine.AddState(restState);
        _stateMachine.AddState(winState);
        _stateMachine.AddState(loseState);

        ICondition buildingDead = new FuncCondition(() => _building != null && _building.IsDead.Value);
        ICondition waveCompleted = new FuncCondition(
            () => _phase == DefendPhase.Wave && _waveSpawnCompleted && _aliveEnemies.Count == 0);
        ICondition hasNextWave = new FuncCondition(() => _currentWaveIndex + 1 < _level.Waves.Count);
        ICondition noNextWave = new FuncCondition(() => _currentWaveIndex + 1 >= _level.Waves.Count);
        ICondition restDone = new FuncCondition(() => _restTimer != null && _restTimer.IsFinished);

        _stateMachine.AddTransition(waveState, loseState, buildingDead);
        _stateMachine.AddTransition(restState, loseState, buildingDead);

        _stateMachine.AddTransition(
            waveState,
            restState,
            new CompositeCondition()
                .Add(waveCompleted)
                .Add(hasNextWave));

        _stateMachine.AddTransition(
            waveState,
            winState,
            new CompositeCondition()
                .Add(waveCompleted)
                .Add(noNextWave));

        _stateMachine.AddTransition(
            restState,
            waveState,
            new CompositeCondition()
                .Add(restDone)
                .Add(hasNextWave));
    }

    private void ProcessInput()
    {
        if (_input.FireDown == false)
        {
            return;
        }

        if (_pointer.TryGetGroundPoint(out Vector3 point) == false)
        {
            return;
        }

        if (_building != null)
        {
            point.y = _building.Transform.position.y;
        }

        if (_phase == DefendPhase.Wave)
        {
            _explosions.Explode(point, _level.ExplosionRadius, _level.ExplosionDamage, _level.ExplosionMask);
            return;
        }

        if (_phase == DefendPhase.Rest)
        {
            TryPlaceMine(point);
        }
    }

    private void UpdateMines(float deltaTime)
    {
        if (_phase != DefendPhase.Wave)
        {
            return;
        }

        for (int i = _mines.Count - 1; i >= 0; i--)
        {
            MineEntity mine = _mines[i];
            mine.Update(deltaTime);

            if (mine.IsReleased)
            {
                _mines.RemoveAt(i);
            }
        }
    }

    private void InitializeWaveSpawn(int waveIndex)
    {
        DefendLevelConfig.WaveConfig wave = _level.Waves[waveIndex];

        if (wave.SpawnInterval <= 0f)
        {
            throw new InvalidOperationException("Wave SpawnInterval must be > 0.");
        }

        _waveEnemiesToSpawn = wave.EnemiesCount;
        _waveEnemiesSpawned = 0;
        _waveSpawnInterval = wave.SpawnInterval;
        _waveSpawnTimer = 0f;
        _waveSpawnCompleted = _waveEnemiesToSpawn <= 0;
        _waveSpawnCompletedLogged = false;
        _waveClearedLogged = false;

        Log(
            $"[Defend] Wave {waveIndex + 1}/{_level.Waves.Count} started. Enemies: {_waveEnemiesToSpawn}, SpawnInterval: {_waveSpawnInterval:0.##}s");
    }

    private void SpawnSingleEnemy()
    {
        Vector2 offset2 = UnityEngine.Random.insideUnitCircle;

        if (offset2.sqrMagnitude < 0.0001f)
        {
            offset2 = Vector2.right;
        }

        offset2 = offset2.normalized * _level.EnemySpawnRadius;
        Vector3 position = _building.Transform.position + new Vector3(offset2.x, 0f, offset2.y);

        Entity enemy = _factory.CreateEnemy(position, _level, _building);
        _aliveEnemies.Add(enemy);
    }

    private void TryPlaceMine(Vector3 point)
    {
        if (_wallet.TrySpend(CurrencyType.Gold, _level.MineCostGold) == false)
        {
            Log(
                $"[Defend] Mine not placed. Need {_level.MineCostGold} gold, current {_wallet.Get(CurrencyType.Gold)}");
            return;
        }

        Entity mineEntity = _factory.CreateMine(point, _level);

        MineEntity mine = new MineEntity(
            _life,
            _explosions,
            _colliders,
            mineEntity,
            mineEntity.Transform,
            _level.MineTriggerRadius,
            _level.MineExplosionRadius,
            _level.MineDamage,
            _level.MineMask);

        _mines.Add(mine);
        Log($"[Defend] Mine placed at {point}");
    }

    private void OnWaveEntered()
    {
        _phase = DefendPhase.Wave;
        _currentWaveIndex++;

        if (_currentWaveIndex < _level.Waves.Count)
        {
            InitializeWaveSpawn(_currentWaveIndex);
        }
    }

    private void OnWaveStateUpdated(float deltaTime)
    {
        if (_phase != DefendPhase.Wave)
        {
            return;
        }

        if (_waveSpawnCompleted == false)
        {
            _waveSpawnTimer -= Mathf.Max(0f, deltaTime);

            while (_waveSpawnTimer <= 0f && _waveEnemiesSpawned < _waveEnemiesToSpawn)
            {
                SpawnSingleEnemy();
                _waveEnemiesSpawned++;
                _waveSpawnTimer += _waveSpawnInterval;
            }

            if (_waveEnemiesSpawned >= _waveEnemiesToSpawn)
            {
                _waveSpawnCompleted = true;
            }
        }

        if (_waveSpawnCompleted && _waveSpawnCompletedLogged == false)
        {
            _waveSpawnCompletedLogged = true;
            Log($"[Defend] Wave {_currentWaveIndex + 1}: all enemies spawned.");
        }

        if (_waveSpawnCompleted && _aliveEnemies.Count == 0 && _waveClearedLogged == false)
        {
            _waveClearedLogged = true;
            Log($"[Defend] Wave {_currentWaveIndex + 1} cleared.");
        }
    }

    private void OnRestEntered()
    {
        _phase = DefendPhase.Rest;
        _restTimer = new CooldownTimer(_level.RestDurationSeconds);
        _restTimer.Reset();
        Log($"[Defend] Rest started for {_level.RestDurationSeconds:0.##}s");
    }

    private void OnRestStateUpdated(float deltaTime)
    {
        _restTimer?.Tick(deltaTime);
    }

    private void OnWinEntered()
    {
        _phase = DefendPhase.Ended;
        _progress.RegisterWin(_level.WinRewardGold);
        Log($"[Defend] WIN. Reward: {_level.WinRewardGold} gold.");
        _flow.OpenMainMenu();
    }

    private void OnLoseEntered()
    {
        _phase = DefendPhase.Ended;
        _progress.RegisterLoss();
        Log("[Defend] LOSE.");
        _flow.OpenMainMenu();
    }

    private void OnEntityReleased(Entity entity)
    {
        _aliveEnemies.Remove(entity);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        Debug.Log(message);
    }
}
