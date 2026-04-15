using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Economy Config", fileName = "EconomyConfig")]
public sealed class EconomyConfig : ScriptableObject
{
    [SerializeField] private int _startGold = 100;
    [SerializeField] private int _startDiamonds = 0;

    [SerializeField] private int _winGold = 10;
    [SerializeField] private int _loseGold = 5;
    [SerializeField] private int _resetCost = 50;

    [Header("Permanent Upgrades")]
    [SerializeField] private int _waveHealCostDiamonds = 5;
    [SerializeField] private float _waveHealPercent = 10f;

    [SerializeField] private int _openingStrikeCostDiamonds = 8;
    [SerializeField] private float _openingStrikeDamagePercent = 35f;
    [SerializeField] private int _openingStrikeTargetsCount = 3;

    [SerializeField] private int _playerExplosionDamageCostDiamonds = 10;
    [SerializeField] private float _playerExplosionDamagePercent = 25f;

    public int StartGold => _startGold;
    public int StartDiamonds => _startDiamonds;
    public int WinGold => _winGold;
    public int LoseGold => _loseGold;
    public int ResetCost => _resetCost;
    public int WaveHealCostDiamonds => _waveHealCostDiamonds;
    public float WaveHealPercent => _waveHealPercent;
    public int OpeningStrikeCostDiamonds => _openingStrikeCostDiamonds;
    public float OpeningStrikeDamagePercent => _openingStrikeDamagePercent;
    public int OpeningStrikeTargetsCount => _openingStrikeTargetsCount;
    public int PlayerExplosionDamageCostDiamonds => _playerExplosionDamageCostDiamonds;
    public float PlayerExplosionDamagePercent => _playerExplosionDamagePercent;
}
