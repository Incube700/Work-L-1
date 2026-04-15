using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Shooter Enemy Config", fileName = "ShooterEnemyConfig")]
public sealed class ShooterEnemyConfig : ScriptableObject
{
    [SerializeField, Min(1f)] private float _health = 30f;
    [SerializeField, Min(0.1f)] private float _moveSpeed = 2f;
    [SerializeField, Min(0.5f)] private float _attackDistance = 5f;
    [SerializeField, Min(0.1f)] private float _attackInterval = 1.5f;
    [SerializeField, Min(0f)] private float _attackDamage = 10f;
    [SerializeField, Min(0.1f)] private float _impactRadius = 0.75f;
    [SerializeField] private string _prefabPath = "Entities/Dummy";

    public float Health => _health;
    public float MoveSpeed => _moveSpeed;
    public float AttackDistance => _attackDistance;
    public float AttackInterval => _attackInterval;
    public float AttackDamage => _attackDamage;
    public float ImpactRadius => _impactRadius;
    public string PrefabPath => _prefabPath;
}