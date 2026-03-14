using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using UnityEngine;

public sealed class EnemySpawner
{
    private readonly DefendLevelConfig _level;
    private readonly DefendEntitiesFactory _factory;
    private readonly Entity _building;
    private readonly Action<Entity> _enemySpawned;

    private int _enemiesToSpawn;
    private int _enemiesSpawned;
    private float _spawnInterval;
    private float _spawnTimer;
    private bool _spawnCompleted;

    public EnemySpawner(
        DefendLevelConfig level,
        DefendEntitiesFactory factory,
        Entity building,
        Action<Entity> enemySpawned)
    {
        _level = level;
        _factory = factory;
        _building = building;
        _enemySpawned = enemySpawned;
    }

    public bool IsCompleted => _spawnCompleted;

    public void StartWave(WaveConfig wave)
    {
        if (wave.SpawnInterval <= 0f)
        {
            throw new InvalidOperationException("Wave SpawnInterval must be > 0.");
        }

        _enemiesToSpawn = wave.EnemiesCount;
        _enemiesSpawned = 0;
        _spawnInterval = wave.SpawnInterval;
        _spawnTimer = 0f;
        _spawnCompleted = _enemiesToSpawn <= 0;
    }

    public void Update(float deltaTime)
    {
        if (_spawnCompleted)
        {
            return;
        }

        _spawnTimer -= Mathf.Max(0f, deltaTime);

        while (_spawnTimer <= 0f && _enemiesSpawned < _enemiesToSpawn)
        {
            SpawnEnemy();
            _enemiesSpawned++;
            _spawnTimer += _spawnInterval;
        }

        if (_enemiesSpawned >= _enemiesToSpawn)
        {
            _spawnCompleted = true;
        }
    }

    private void SpawnEnemy()
    {
        Vector2 offset2 = UnityEngine.Random.insideUnitCircle;

        if (offset2.sqrMagnitude < 0.0001f)
        {
            offset2 = Vector2.right;
        }

        offset2 = offset2.normalized * _level.EnemyConfig.SpawnRadius;
        Vector3 position = _building.Transform.position + new Vector3(offset2.x, 0f, offset2.y);

        Entity enemy = _factory.CreateEnemy(position, _level, _building);
        _enemySpawned?.Invoke(enemy);
    }
}