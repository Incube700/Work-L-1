using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Mine Config", fileName = "MineConfig")]
public sealed class MineConfig : ScriptableObject
{
    [SerializeField, Min(1)] private int _costGold = 10;
    [SerializeField, Min(0f)] private float _triggerRadius = 1.5f;
    [SerializeField, Min(0f)] private float _explosionRadius = 2.5f;
    [SerializeField, Min(0f)] private float _damage = 60f;
    [SerializeField] private string _prefabPath = "Entities/Dummy";
    [SerializeField] private LayerMask _mask = ~0;

    public int CostGold => _costGold;
    public float TriggerRadius => _triggerRadius;
    public float ExplosionRadius => _explosionRadius;
    public float Damage => _damage;
    public string PrefabPath => _prefabPath;
    public LayerMask Mask => _mask;
}