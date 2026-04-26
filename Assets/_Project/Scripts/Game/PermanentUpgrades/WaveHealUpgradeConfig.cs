using UnityEngine;

[CreateAssetMenu(
    menuName = "Configs/Permanent Upgrades/Wave Heal Upgrade Config",
    fileName = "WaveHealUpgradeConfig")]
public sealed class WaveHealUpgradeConfig : PermanentUpgradeConfigBase
{
    [SerializeField, Min(0f)] private float _healPercent = 10f;

    public float HealPercent => _healPercent;
}