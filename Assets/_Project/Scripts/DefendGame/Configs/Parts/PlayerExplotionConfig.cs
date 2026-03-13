using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Player Explosion Config", fileName = "PlayerExplosionConfig")]
public sealed class PlayerExplosionConfig : ScriptableObject
{
    [SerializeField, Min(0f)] private float _radius = 2.5f;
    [SerializeField, Min(0f)] private float _damage = 40f;
    [SerializeField] private LayerMask _mask = ~0;

    public float Radius => _radius;
    public float Damage => _damage;
    public LayerMask Mask => _mask;
}