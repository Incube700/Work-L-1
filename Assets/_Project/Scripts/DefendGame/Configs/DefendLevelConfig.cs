using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Defend Level Config", fileName = "DefendLevelConfig")]
public sealed class DefendLevelConfig : ScriptableObject
{
    [Header("Level Flow")]
    [SerializeField, Min(1)] private int _winRewardGold = 20;
    [SerializeField, Min(0.1f)] private float _restDurationSeconds = 2f;

    [Header("Parts")]
    [SerializeField] private BuildingConfig _buildingConfig;
    [SerializeField] private EnemyConfig _enemyConfig;
    [SerializeField] private PlayerExplosionConfig _playerExplosionConfig;
    [SerializeField] private MineConfig _mineConfig;

    [Header("Waves")]
    [SerializeField] private List<WaveConfig> _waves = new List<WaveConfig>();

    public int WinRewardGold => _winRewardGold;
    public float RestDurationSeconds => _restDurationSeconds;

    public BuildingConfig BuildingConfig => _buildingConfig;
    public EnemyConfig EnemyConfig => _enemyConfig;
    public PlayerExplosionConfig PlayerExplosionConfig => _playerExplosionConfig;
    public MineConfig MineConfig => _mineConfig;
    public IReadOnlyList<WaveConfig> Waves => _waves;

    // ВРЕМЕННЫЕ PROXY-СВОЙСТВА ДЛЯ СОВМЕСТИМОСТИ.
    // Следующим шагом переведём код на новые child-config references
    // и эти свойства удалим.

    public float BuildingHealth => _buildingConfig.Health;
    public string BuildingPrefabPath => _buildingConfig.PrefabPath;

    public float EnemyHealth => _enemyConfig.Health;
    public float EnemyMoveSpeed => _enemyConfig.MoveSpeed;
    public float EnemySpawnRadius => _enemyConfig.SpawnRadius;
    public float EnemyExplodeDistance => _enemyConfig.ExplodeDistance;
    public float EnemyExplodeDamage => _enemyConfig.ExplodeDamage;
    public string EnemyPrefabPath => _enemyConfig.PrefabPath;

    public float ExplosionRadius => _playerExplosionConfig.Radius;
    public float ExplosionDamage => _playerExplosionConfig.Damage;
    public LayerMask ExplosionMask => _playerExplosionConfig.Mask;

    public int MineCostGold => _mineConfig.CostGold;
    public float MineTriggerRadius => _mineConfig.TriggerRadius;
    public float MineExplosionRadius => _mineConfig.ExplosionRadius;
    public float MineDamage => _mineConfig.Damage;
    public string MinePrefabPath => _mineConfig.PrefabPath;
    public LayerMask MineMask => _mineConfig.Mask;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_winRewardGold < 1)
        {
            _winRewardGold = 1;
        }

        if (_restDurationSeconds <= 0f)
        {
            _restDurationSeconds = 0.1f;
        }

        if (_waves == null)
        {
            _waves = new List<WaveConfig>();
        }
    }
#endif
}