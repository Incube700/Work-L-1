using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Turret Config", fileName = "TurretConfig")]
public sealed class TurretConfig : ScriptableObject
{
    [SerializeField, Min(1)] private int _costGold = 25;
    [SerializeField, Min(0.1f)] private float _radius = 6f;
    [SerializeField, Min(0.1f)] private float _attackInterval = 1f;
    [SerializeField] private string _prefabPath = "Entities/Dummy";
    [SerializeField] private LayerMask _mask = ~0;
    [SerializeField, Min(0f)] private float _rotateSpeed = 360f;
    [SerializeField] private ProjectileConfig _projectileConfig;

    public int CostGold => _costGold;
    public float Radius => _radius;
    public float AttackInterval => _attackInterval;
    public string PrefabPath => _prefabPath;
    public LayerMask Mask => _mask;
    public float RotateSpeed => _rotateSpeed;
    public ProjectileConfig ProjectileConfig => _projectileConfig;
}