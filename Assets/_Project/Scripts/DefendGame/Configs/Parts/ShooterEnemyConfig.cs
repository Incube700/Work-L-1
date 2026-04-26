using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Shooter Enemy Config", fileName = "ShooterEnemyConfig")]
public sealed class ShooterEnemyConfig : EnemyConfigBase
{
    [SerializeField, Min(0.5f)] private float _attackDistance = 5f;
    [SerializeField, Min(0.1f)] private float _attackInterval = 1.5f;
    [SerializeField] private ProjectileConfig _projectileConfig;

    public float AttackDistance => _attackDistance;
    public float AttackInterval => _attackInterval;
    public ProjectileConfig ProjectileConfig => _projectileConfig;
}