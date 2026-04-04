using System;
using UnityEngine;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;

public sealed class DefendInputHandler
{
    private readonly IInputService _input;
    private readonly IPointerService _pointer;
    private readonly MinePlacementService _minePlacementService;

    public event Action<Vector3> AimPointChanged;

    public event Action<Vector3> PlayerAttacked;

    public DefendInputHandler(
        IInputService input,
        IPointerService pointer,
        MinePlacementService minePlacementService)
    {
        _input = input;
        _pointer = pointer;
        _minePlacementService = minePlacementService;
    }

    public void Update(DefendPhase phase, float buildingY)
    {
        if (_pointer.TryGetGroundPoint(out Vector3 point))
        {
            point.y = buildingY;
            AimPointChanged?.Invoke(point);
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
            PlayerAttacked?.Invoke(point);
            return;
        }

        if (phase == DefendPhase.Rest)
        {
            _minePlacementService.TryPlace(point);
        }
    }
}