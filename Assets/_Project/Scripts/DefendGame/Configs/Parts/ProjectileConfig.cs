using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Projectile Config", fileName = "ProjectileConfig")]
public sealed class ProjectileConfig : ScriptableObject
{
    [SerializeField] private string _prefabPath = "Prefabs/ProjectileFireball";
    [SerializeField, Min(0.1f)] private float _speed = 10f;
    [SerializeField, Min(0.1f)] private float _lifeTime = 3f;
    [SerializeField, Min(0.01f)] private float _hitDistance = 0.2f;
    [SerializeField, Min(0.01f)] private float _collisionRadius = 0.25f;
    [SerializeField, Min(0f)] private float _damage = 10f;
    [SerializeField, Min(0.01f)] private float _explosionRadius = 0.5f;
    [SerializeField] private LayerMask _mask = ~0;

    public string PrefabPath => _prefabPath;
    public float Speed => _speed;
    public float LifeTime => _lifeTime;
    public float HitDistance => _hitDistance;
    public float CollisionRadius => _collisionRadius;
    public float Damage => _damage;
    public float ExplosionRadius => _explosionRadius;
    public LayerMask Mask => _mask;
}