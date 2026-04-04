using UnityEngine;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;

public sealed class DefendInputHandler
{
    private readonly IInputService _input;
    private readonly IPointerService _pointer;
    private readonly MinePlacementService _minePlacementService;
    private readonly BuildingCombatService _buildingCombatService;

    public DefendInputHandler(
        IInputService input,
        IPointerService pointer,
        MinePlacementService minePlacementService,
        BuildingCombatService buildingCombatService)
    {
        _input = input;
        _pointer = pointer;
        _minePlacementService = minePlacementService;
        _buildingCombatService = buildingCombatService;
    }

    public void Update(DefendPhase phase, float buildingY)
    {
        if (_pointer.TryGetGroundPoint(out Vector3 point))
        {
            point.y = buildingY;
            _buildingCombatService.UpdateAimPoint(point);
        }

        if (_input.FireDown == false)
        {
            return;
        }

        if (_pointer.TryGetGroundPoint(out point) == false)
        {
            return;
        }

        point.y = buildingY;

        if (phase == DefendPhase.Wave)
        {
            _buildingCombatService.TryAttack(point);
            return;
        }

        if (phase == DefendPhase.Rest)
        {
            _minePlacementService.TryPlace(point);
        }
    }
}