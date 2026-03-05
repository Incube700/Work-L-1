using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Defend Level Config", fileName = "DefendLevelConfig")]
public sealed class DefendLevelConfig : ScriptableObject
{
    [Header("Building")]
    [SerializeField, Min(1f)] private float _buildingHealth = 200f;
    [SerializeField] private string _buildingPrefabPath = "Entities/Dummy";
    [SerializeField, Min(1)] private int _winRewardGold = 20;

    [Header("Enemy")]
    [SerializeField, Min(1f)] private float _enemyHealth = 40f;
    [SerializeField, Min(0.1f)] private float _enemyMoveSpeed = 2.5f;
    [SerializeField, Min(0f)] private float _enemySpawnRadius = 10f;
    [SerializeField, Min(0f)] private float _enemyExplodeDistance = 1.25f;
    [SerializeField, Min(0f)] private float _enemyExplodeDamage = 25f;
    [SerializeField] private string _enemyPrefabPath = "Entities/Dummy";

    [Header("Explosion (Wave phase)")]
    [SerializeField, Min(0f)] private float _explosionRadius = 2.5f;
    [SerializeField, Min(0f)] private float _explosionDamage = 40f;
    [SerializeField] private LayerMask _explosionMask = ~0;

    [Header("Mine (Rest phase)")]
    [SerializeField, Min(1)] private int _mineCostGold = 10;
    [SerializeField, Min(0.1f)] private float _restDurationSeconds = 2f;
    [SerializeField, Min(0f)] private float _mineTriggerRadius = 1.5f;
    [SerializeField, Min(0f)] private float _mineExplosionRadius = 2.5f;
    [SerializeField, Min(0f)] private float _mineDamage = 60f;
    [SerializeField] private string _minePrefabPath = "Entities/Dummy";
    [SerializeField] private LayerMask _mineMask = ~0;

    [Header("Waves")]
    [SerializeField] private List<WaveConfig> _waves = new List<WaveConfig>();

    public float BuildingHealth => _buildingHealth;
    public string BuildingPrefabPath => _buildingPrefabPath;
    public int WinRewardGold => _winRewardGold;
    public float EnemyHealth => _enemyHealth;
    public float EnemyMoveSpeed => _enemyMoveSpeed;
    public float EnemySpawnRadius => _enemySpawnRadius;
    public float EnemyExplodeDistance => _enemyExplodeDistance;
    public float EnemyExplodeDamage => _enemyExplodeDamage;
    public string EnemyPrefabPath => _enemyPrefabPath;
    public float ExplosionRadius => _explosionRadius;
    public float ExplosionDamage => _explosionDamage;
    public LayerMask ExplosionMask => _explosionMask;
    public int MineCostGold => _mineCostGold;
    public float RestDurationSeconds => _restDurationSeconds;
    public float MineTriggerRadius => _mineTriggerRadius;
    public float MineExplosionRadius => _mineExplosionRadius;
    public float MineDamage => _mineDamage;
    public string MinePrefabPath => _minePrefabPath;
    public LayerMask MineMask => _mineMask;
    public IReadOnlyList<WaveConfig> Waves => _waves;

    [Serializable]
    public sealed class WaveConfig
    {
        [SerializeField, Min(1)] private int _enemiesCount = 5;
        [SerializeField, Min(0.05f)] private float _spawnInterval = 0.5f;

        public int EnemiesCount => _enemiesCount;
        public float SpawnInterval => _spawnInterval;

        public void Validate()
        {
            if (_enemiesCount < 1)
            {
                _enemiesCount = 1;
            }

            if (_spawnInterval <= 0f)
            {
                _spawnInterval = 0.5f;
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_winRewardGold < 1)
        {
            _winRewardGold = 1;
        }

        if (_mineCostGold < 1)
        {
            _mineCostGold = 1;
        }

        if (_restDurationSeconds <= 0f)
        {
            _restDurationSeconds = 0.1f;
        }

        if (_waves == null)
        {
            _waves = new List<WaveConfig>();
        }

        for (int i = 0; i < _waves.Count; i++)
        {
            _waves[i]?.Validate();
        }
    }
#endif
}
