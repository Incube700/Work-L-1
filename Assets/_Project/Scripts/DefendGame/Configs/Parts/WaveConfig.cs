using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Wave Config", fileName = "WaveConfig")]
public sealed class WaveConfig : ScriptableObject
{
    [SerializeField] private EnemyConfigBase _enemyConfig;
    [SerializeField, Min(1)] private int _enemiesCount = 5;
    [SerializeField, Min(0.05f)] private float _spawnInterval = 0.5f;
    [SerializeField, Min(0f)] private float _SpawnRadius = 10f;

    public EnemyConfigBase EnemyConfig => _enemyConfig;
    public int EnemiesCount => _enemiesCount;
    public float SpawnInterval => _spawnInterval;
    public float SpawnRadius => _SpawnRadius;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_enemiesCount < 1)
        {
            _enemiesCount = 1;
        }

        if (_spawnInterval <= 0f)
        {
            _spawnInterval = 0.5f;
        }
        if (_SpawnRadius < 0f)
        {
            _SpawnRadius = 10f;
        }
    }
#endif
}