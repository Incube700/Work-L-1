using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Enemy Config", fileName = "EnemyConfig")]
public sealed class EnemyConfig : ScriptableObject
{
    [SerializeField, Min(1f)] private float _health = 40f;
    [SerializeField, Min(0.1f)] private float _moveSpeed = 2.5f;
    [SerializeField, Min(0f)] private float _spawnRadius = 10f;
    [SerializeField, Min(0f)] private float _explodeDistance = 1.25f;
    [SerializeField, Min(0f)] private float _explodeDamage = 25f;
    [SerializeField] private string _prefabPath = "Entities/Dummy";

    public float Health => _health;
    public float MoveSpeed => _moveSpeed;
    public float SpawnRadius => _spawnRadius;
    public float ExplodeDistance => _explodeDistance;
    public float ExplodeDamage => _explodeDamage;
    public string PrefabPath => _prefabPath;
}