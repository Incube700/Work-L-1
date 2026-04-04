using UnityEngine;

public sealed class DefendGameplaySceneData
{
    public DefendGameplaySceneData(
        Vector3 buildingSpawnPoint,
        LayerMask groundMask)
    {
        BuildingSpawnPoint = buildingSpawnPoint;
        GroundMask = groundMask;
    }

    public Vector3 BuildingSpawnPoint { get; }
    public LayerMask GroundMask { get; }
}