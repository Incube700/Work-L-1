using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Shooter Enemy Config", fileName = "ShooterEnemyConfig")]
public sealed class ShooterEnemyConfig : EnemyConfigBase
{
    [SerializeField, Min(0.5f)] private float _attackDistance = 5f;
    [SerializeField, Min(0.1f)] private float _attackInterval = 1.5f;
    [SerializeField, Min(0f)] private float _attackDamage = 10f;
    [SerializeField, Min(0.1f)] private float _impactRadius = 0.75f;
    [SerializeField] private string _projectilePrefabPath = "Prefabs/Fireball";

    public float AttackDistance => _attackDistance;
    public float AttackInterval => _attackInterval;
    public float AttackDamage => _attackDamage;
    public float ImpactRadius => _impactRadius;
    public string ProjectilePrefabPath => _projectilePrefabPath;
}