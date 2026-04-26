using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Player Explosion Config", fileName = "PlayerExplosionConfig")]
public sealed class PlayerExplosionConfig : ScriptableObject
{
    [SerializeField] private ProjectileConfig _projectileConfig;

    public ProjectileConfig ProjectileConfig => _projectileConfig;
}