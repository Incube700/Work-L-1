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