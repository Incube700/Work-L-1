using UnityEngine;

[CreateAssetMenu(
    menuName = "Configs/Permanent Upgrades/Opening Strike Upgrade Config",
    fileName = "OpeningStrikeUpgradeConfig")]
public sealed class OpeningStrikeUpgradeConfig : PermanentUpgradeConfigBase
{
    [SerializeField, Min(0f)] private float _damagePercent = 35f;
    [SerializeField, Min(1)] private int _targetsCount = 3;

    public float DamagePercent => _damagePercent;
    public int TargetsCount => _targetsCount;
}