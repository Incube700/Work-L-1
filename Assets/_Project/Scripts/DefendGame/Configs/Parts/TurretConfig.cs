using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Turret Config", fileName = "TurretConfig")]
public sealed class TurretConfig : ScriptableObject
{
    [SerializeField, Min(1)] private int _costGold = 25;
    [SerializeField, Min(0.1f)] private float _radius = 6f;
    [SerializeField, Min(0.1f)] private float _attackInterval = 1f;
    [SerializeField, Min(0f)] private float _damage = 10f;
    [SerializeField, Min(0.1f)] private float _impactRadius = 0.5f;
    [SerializeField] private string _prefabPath = "Entities/Dummy";
    [SerializeField] private LayerMask _mask = ~0;
    [SerializeField, Min(0f)] private float _rotateSpeed = 360f;

    [SerializeField] private string _projectilePrefabPath = "Prefabs/TurretFireball";
    [SerializeField, Min(0.1f)] private float _projectileSpeed = 10f;
    [SerializeField, Min(0.1f)] private float _projectileLifeTime = 3f;
    [SerializeField, Min(0.01f)] private float _projectileHitDistance = 0.2f;

    public int CostGold => _costGold;
    public float Radius => _radius;
    public float AttackInterval => _attackInterval;
    public float Damage => _damage;
    public float ImpactRadius => _impactRadius;
    public string PrefabPath => _prefabPath;
    public LayerMask Mask => _mask;
    public float RotateSpeed => _rotateSpeed;

    public string ProjectilePrefabPath => _projectilePrefabPath;
    public float ProjectileSpeed => _projectileSpeed;
    public float ProjectileLifeTime => _projectileLifeTime;
    public float ProjectileHitDistance => _projectileHitDistance;
}