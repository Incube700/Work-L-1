using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Economy Config", fileName = "EconomyConfig")]
public sealed class EconomyConfig : ScriptableObject
{
    [field: SerializeField] public int StartGold { get; private set; } = 100;
    [field: SerializeField] public int WinGold { get; private set; } = 10;
    [field: SerializeField] public int LoseGold { get; private set; } = 5;
    [field: SerializeField] public int ResetCost { get; private set; } = 50;
}