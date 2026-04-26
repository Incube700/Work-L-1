using UnityEngine;

[CreateAssetMenu(
    menuName = "Configs/Permanent Upgrades/Player Explosion Damage Upgrade Config",
    fileName = "PlayerExplosionDamageUpgradeConfig")]
public sealed class PlayerExplosionDamageUpgradeConfig : PermanentUpgradeConfigBase
{
    [SerializeField, Min(0f)] private float _damagePercent = 25f;

    public float DamagePercent => _damagePercent;
}