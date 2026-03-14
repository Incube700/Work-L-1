using UnityEngine;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;

public sealed class DefendInputHandler
{
    private readonly IInputService _input;
    private readonly IPointerService _pointer;
    private readonly ExplosionService _explosions;
    private readonly MinePlacementService _minePlacementService;
    private readonly DefendLevelConfig _level;

    public DefendInputHandler(
        IInputService input,
        IPointerService pointer,
        ExplosionService explosions,
        MinePlacementService minePlacementService,
        DefendLevelConfig level)
    {
        _input = input;
        _pointer = pointer;
        _explosions = explosions;
        _minePlacementService = minePlacementService;
        _level = level;
    }

    public void Update(DefendPhase phase, float buildingY)
    {
        if (_input.FireDown == false)
        {
            return;
        }

        if (_pointer.TryGetGroundPoint(out Vector3 point) == false)
        {
            return;
        }

        point.y = buildingY;

        if (phase == DefendPhase.Wave)
        {
            _explosions.Explode(point, _level.PlayerExplosionConfig.Radius, _level.PlayerExplosionConfig.Damage, _level.PlayerExplosionConfig.Mask);
            return;
        }

        if (phase == DefendPhase.Rest)
        {
            _minePlacementService.TryPlace(point);
        }
    }
}