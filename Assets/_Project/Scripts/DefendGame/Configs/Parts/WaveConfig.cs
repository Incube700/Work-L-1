using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Wave Config", fileName = "WaveConfig")]
public sealed class WaveConfig : ScriptableObject
{
    [SerializeField, Min(1)] private int _enemiesCount = 5;
    [SerializeField, Min(0.05f)] private float _spawnInterval = 0.5f;

    public int EnemiesCount => _enemiesCount;
    public float SpawnInterval => _spawnInterval;

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
    }
#endif
}