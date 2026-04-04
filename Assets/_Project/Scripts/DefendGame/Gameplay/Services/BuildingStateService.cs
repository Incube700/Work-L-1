using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;

public sealed class BuildingStateService : IDisposable
{
    private Entity _building;
    private IReadOnlyReactiveVariable<float> _currentHealth;

    public event Action BuildingChanged;
    public event Action HealthChanged;

    public Entity Building => _building;

    public bool HasBuilding => _building != null;
    public bool IsDead => _building != null && _building.IsDead.Value;
    public float CurrentHealth => _building != null ? _building.CurrentHealth.Value : 0f;
    public float MaxHealth => _building != null ? _building.MaxHealth.Value : 0f;

    public void SetBuilding(Entity building)
    {
        if (building == null)
        {
            throw new ArgumentNullException(nameof(building));
        }

        UnsubscribeFromBuilding();

        _building = building;
        _currentHealth = _building.CurrentHealth;
        _currentHealth.Changed += OnHealthChanged;

        BuildingChanged?.Invoke();
        HealthChanged?.Invoke();
    }

    public void Dispose()
    {
        UnsubscribeFromBuilding();

        _building = null;
        BuildingChanged = null;
        HealthChanged = null;
    }

    private void OnHealthChanged()
    {
        HealthChanged?.Invoke();
    }

    private void UnsubscribeFromBuilding()
    {
        if (_currentHealth == null)
        {
            return;
        }

        _currentHealth.Changed -= OnHealthChanged;
        _currentHealth = null;
    }
}