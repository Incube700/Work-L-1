using System;

public sealed class PlacementSelectionService
{
    public event Action SelectedTypeChanged;

    public PlaceableType SelectedType { get; private set; } = PlaceableType.Mine;

    public void Select(PlaceableType type)
    {
        if (SelectedType == type)
        {
            return;
        }

        SelectedType = type;
        SelectedTypeChanged?.Invoke();
    }
}