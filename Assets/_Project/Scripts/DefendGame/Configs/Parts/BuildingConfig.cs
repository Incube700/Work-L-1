using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Building Config", fileName = "BuildingConfig")]
public sealed class BuildingConfig : ScriptableObject
{
    [SerializeField, Min(1f)] private float _health = 200f;
    [SerializeField] private string _prefabPath = "Entities/Dummy";

    public float Health => _health;
    public string PrefabPath => _prefabPath;
}