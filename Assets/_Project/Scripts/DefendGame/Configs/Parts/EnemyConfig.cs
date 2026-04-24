using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Enemy Config", fileName = "EnemyConfig")]
public sealed class EnemyConfig : EnemyConfigBase
{
    [SerializeField, Min(0f)] private float _explodeDistance = 1.25f;
    [SerializeField, Min(0f)] private float _explodeDamage = 25f;

    public float ExplodeDistance => _explodeDistance;
    public float ExplodeDamage => _explodeDamage;
}