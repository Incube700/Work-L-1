using UnityEngine;

public abstract class EnemyConfigBase : ScriptableObject
{
    [SerializeField, Min(1f)] private float _health = 40f;
    [SerializeField, Min(0.1f)] private float _moveSpeed = 2.5f;
    [SerializeField, Min(0f)] private float _spawnRadius = 10f;
    [SerializeField] private string _prefabPath = "Entities/Dummy";

    public float Health => _health;
    public float MoveSpeed => _moveSpeed;
    public float SpawnRadius => _spawnRadius;
    public string PrefabPath => _prefabPath;
}