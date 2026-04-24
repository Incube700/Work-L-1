using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using UnityEngine;

public sealed class EnemySpawner
{
    private readonly DefendLevelConfig _level;
    private readonly DefendEntitiesFactory _factory;
    private readonly BuildingStateService _buildingStateService;
    private readonly Action<Entity> _enemySpawned;

    private WaveConfig _currentWave;
    private int _enemiesToSpawn;
    private int _enemiesSpawned;
    private IntervalTimer _spawnTimer;
    private bool _spawnCompleted;

    public EnemySpawner(
        DefendLevelConfig level,
        DefendEntitiesFactory factory,
        BuildingStateService buildingStateService,
        Action<Entity> enemySpawned)
    {
        _level = level;
        _factory = factory;
        _buildingStateService = buildingStateService;
        _enemySpawned = enemySpawned;
    }

    public bool IsCompleted => _spawnCompleted;

    public void StartWave(WaveConfig wave)
    {
        if (wave == null)
        {
            throw new InvalidOperationException("Wave is null.");
        }

        if (wave.EnemyConfig == null)
        {
            throw new InvalidOperationException("Wave EnemyConfig is not assigned.");
        }

        if (wave.SpawnInterval <= 0f)
        {
            throw new InvalidOperationException("Wave SpawnInterval must be > 0.");
        }

        _currentWave = wave;
        _enemiesToSpawn = wave.EnemiesCount;
        _enemiesSpawned = 0;
        _spawnTimer = new IntervalTimer(wave.SpawnInterval);
        _spawnTimer.Reset();
        _spawnCompleted = _enemiesToSpawn <= 0;
    }

    public void Update(float deltaTime)
    {
        if (_spawnCompleted)
        {
            return;
        }

        int spawnCount = _spawnTimer.Tick(deltaTime);

        for (int i = 0; i < spawnCount; i++)
        {
            if (_enemiesSpawned >= _enemiesToSpawn)
            {
                break;
            }

            SpawnEnemy();
            _enemiesSpawned++;
        }

        if (_enemiesSpawned >= _enemiesToSpawn)
        {
            _spawnCompleted = true;
        }
    }

    private void SpawnEnemy()
    {
        if (_buildingStateService.HasBuilding == false)
        {
            throw new InvalidOperationException("Building is not initialized.");
        }

        if (_currentWave == null)
        {
            throw new InvalidOperationException("Current wave is not initialized.");
        }

        Entity building = _buildingStateService.Building;
        EnemyConfigBase enemyConfig = _currentWave.EnemyConfig;

        Vector2 offset2 = UnityEngine.Random.insideUnitCircle;

        if (offset2.sqrMagnitude < 0.0001f)
        {
            offset2 = Vector2.right;
        }

        offset2 = offset2.normalized * enemyConfig.SpawnRadius;
        Vector3 position = building.Transform.position + new Vector3(offset2.x, 0f, offset2.y);

        Entity enemy = _factory.CreateEnemy(position, enemyConfig, building);
        _enemySpawned?.Invoke(enemy);
    }
}