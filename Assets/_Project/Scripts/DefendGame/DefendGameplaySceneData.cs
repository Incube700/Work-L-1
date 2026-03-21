using UnityEngine;

public sealed class DefendGameplaySceneData
{
    public DefendGameplaySceneData(
        Vector3 buildingSpawnPoint,
        LayerMask groundMask,
        DefendGameplayScreenView screenView)
    {
        BuildingSpawnPoint = buildingSpawnPoint;
        GroundMask = groundMask;
        ScreenView = screenView;
    }

    public Vector3 BuildingSpawnPoint { get; }
    public LayerMask GroundMask { get; }
    public DefendGameplayScreenView ScreenView { get; }
}