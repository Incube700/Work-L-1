using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Economy Config", fileName = "EconomyConfig")]
public sealed class EconomyConfig : ScriptableObject
{
    [SerializeField] private int _startGold = 100;
    [SerializeField] private int _startDiamonds = 0;

    [SerializeField] private int _winGold = 10;
    [SerializeField] private int _loseGold = 5;
    [SerializeField] private int _resetCost = 50;

    public int StartGold => _startGold;
    public int StartDiamonds => _startDiamonds;
    public int WinGold => _winGold;
    public int LoseGold => _loseGold;
    public int ResetCost => _resetCost;
}