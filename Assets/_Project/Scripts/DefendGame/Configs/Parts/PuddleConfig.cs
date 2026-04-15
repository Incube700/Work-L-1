using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Puddle Config", fileName = "PuddleConfig")]
public sealed class PuddleConfig : ScriptableObject
{
    [SerializeField, Min(1)] private int _costGold = 15;
    [SerializeField, Min(0.1f)] private float _radius = 2f;
    [SerializeField, Min(0.1f)] private float _tickInterval = 1f;
    [SerializeField, Min(0f)] private float _damagePerTick = 10f;
    [SerializeField] private string _prefabPath = "Entities/Dummy";
    [SerializeField] private LayerMask _mask = ~0;

    public int CostGold => _costGold;
    public float Radius => _radius;
    public float TickInterval => _tickInterval;
    public float DamagePerTick => _damagePerTick;
    public string PrefabPath => _prefabPath;
    public LayerMask Mask => _mask;
}