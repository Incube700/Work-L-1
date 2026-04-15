using System;
using UnityEngine;

public sealed class PlacementService
{
    private readonly PlacementSelectionService _selectionService;
    private readonly MinePlacementService _minePlacementService;
    private readonly TurretPlacementService _turretPlacementService;
    private readonly PuddlePlacementService _puddlePlacementService;

    public PlacementService(
        PlacementSelectionService selectionService,
        MinePlacementService minePlacementService,
        TurretPlacementService turretPlacementService,
        PuddlePlacementService puddlePlacementService)
    {
        _selectionService = selectionService ?? throw new ArgumentNullException(nameof(selectionService));
        _minePlacementService = minePlacementService ?? throw new ArgumentNullException(nameof(minePlacementService));
        _turretPlacementService = turretPlacementService ?? throw new ArgumentNullException(nameof(turretPlacementService));
        _puddlePlacementService = puddlePlacementService ?? throw new ArgumentNullException(nameof(puddlePlacementService));
    }

    public bool TryPlace(Vector3 point)
    {
        switch (_selectionService.SelectedType)
        {
            case PlaceableType.Mine:
                return _minePlacementService.TryPlace(point);

            case PlaceableType.Turret:
                return _turretPlacementService.TryPlace(point);

            case PlaceableType.Puddle:
                return _puddlePlacementService.TryPlace(point);

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}