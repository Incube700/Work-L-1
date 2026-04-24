using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Defend Level Config", fileName = "DefendLevelConfig")]
public sealed class DefendLevelConfig : ScriptableObject
{
    [Header("Level Flow")]
    [SerializeField, Min(1)] private int _winRewardGold = 20;
    [SerializeField, Min(0)] private int _winRewardDiamonds = 1;
    [SerializeField, Min(0.1f)] private float _restDurationSeconds = 5f;

    [Header("Parts")]
    [SerializeField] private BuildingConfig _buildingConfig;
    [SerializeField] private PlayerExplosionConfig _playerExplosionConfig;
    [SerializeField] private MineConfig _mineConfig;
    [SerializeField] private TurretConfig _turretConfig;
    [SerializeField] private PuddleConfig _puddleConfig;

    [Header("Waves")]
    [SerializeField] private List<WaveConfig> _waves = new List<WaveConfig>();

    public int WinRewardGold => _winRewardGold;
    public int WinRewardDiamonds => _winRewardDiamonds;
    public float RestDurationSeconds => _restDurationSeconds;

    public BuildingConfig BuildingConfig => _buildingConfig;
    public PlayerExplosionConfig PlayerExplosionConfig => _playerExplosionConfig;
    public MineConfig MineConfig => _mineConfig;
    public TurretConfig TurretConfig => _turretConfig;
    public PuddleConfig PuddleConfig => _puddleConfig;
    public IReadOnlyList<WaveConfig> Waves => _waves;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_winRewardGold < 1)
        {
            _winRewardGold = 1;
        }

        if (_winRewardDiamonds < 0)
        {
            _winRewardDiamonds = 0;
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