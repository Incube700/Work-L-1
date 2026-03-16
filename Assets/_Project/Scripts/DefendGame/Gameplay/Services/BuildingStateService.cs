using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;

public sealed class BuildingStateService
{
    private Entity _building;

    public Entity Building => _building;

    public bool HasBuilding => _building != null;
    public bool IsDead => _building != null && _building.IsDead.Value;
    public float CurrentHealth => _building != null ? _building.CurrentHealth.Value : 0f;
    public float MaxHealth => _building != null ? _building.MaxHealth.Value : 0f;

    public void SetBuilding(Entity building)
    {
        _building = building ?? throw new ArgumentNullException(nameof(building));
    }
}