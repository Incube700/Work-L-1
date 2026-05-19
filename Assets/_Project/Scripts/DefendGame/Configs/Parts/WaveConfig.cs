using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Configs/Defend Game/Wave Config", fileName = "WaveConfig")]
public sealed class WaveConfig : ScriptableObject
{
    [SerializeField] private EnemyConfigBase _enemyConfig;
    [SerializeField, Min(1)] private int _enemiesCount = 5;
    [SerializeField, Min(0.05f)] private float _spawnInterval = 0.5f;
    [FormerlySerializedAs("_SpawnRadius")]
    [SerializeField, Min(0f)] private float _spawnRadius = 10f;

    public EnemyConfigBase EnemyConfig => _enemyConfig;
    public int EnemiesCount => _enemiesCount;
    public float SpawnInterval => _spawnInterval;
    public float SpawnRadius => _spawnRadius;

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
        if (_spawnRadius < 0f)
        {
            _spawnRadius = 10f;
        }
    }
#endif
}
