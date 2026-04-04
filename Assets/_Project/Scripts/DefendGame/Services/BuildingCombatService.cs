using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

public sealed class BuildingCombatService
{
    private const float MinDirectionSqrMagnitude = 0.0001f;

    private readonly BuildingStateService _buildingStateService;

    public event Action<Vector3> AttackPerformed;

    public BuildingCombatService(BuildingStateService buildingStateService)
    {
        _buildingStateService = buildingStateService ?? throw new ArgumentNullException(nameof(buildingStateService));
    }

    public void UpdateAimPoint(Vector3 targetPoint)
    {
        if (_buildingStateService.HasBuilding == false)
        {
            return;
        }

        if (_buildingStateService.IsDead)
        {
            return;
        }

        Entity building = _buildingStateService.Building;

        if (building.HasComponent<RotationDirection>() == false)
        {
            return;
        }

        Vector3 direction = targetPoint - building.Transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < MinDirectionSqrMagnitude)
        {
            return;
        }

        building.RotationDirection.Value = direction.normalized;
    }

    public bool TryAttack(Vector3 targetPoint)
    {
        if (_buildingStateService.HasBuilding == false)
        {
            return false;
        }

        if (_buildingStateService.IsDead)
        {
            return false;
        }

        UpdateAimPoint(targetPoint);

        AttackPerformed?.Invoke(targetPoint);
        return true;
    }
}