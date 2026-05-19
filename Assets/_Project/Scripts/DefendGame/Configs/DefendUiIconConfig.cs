using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/UI Icon Config", fileName = "DefendUiIconConfig")]
public sealed class DefendUiIconConfig : ScriptableObject
{
    [SerializeField] private CurrencyPresentationEntry[] _currencyEntries = Array.Empty<CurrencyPresentationEntry>();
    [SerializeField] private PlaceablePresentationEntry[] _placeableEntries = Array.Empty<PlaceablePresentationEntry>();
    [SerializeField] private PhasePresentationEntry[] _phaseEntries = Array.Empty<PhasePresentationEntry>();
    [SerializeField] private EnemyPresentationEntry[] _enemyEntries = Array.Empty<EnemyPresentationEntry>();

    public bool TryGetCurrency(CurrencyType type, out string displayName, out Sprite icon)
    {
        if (_currencyEntries == null)
        {
            displayName = string.Empty;
            icon = null;
            return false;
        }

        for (int i = 0; i < _currencyEntries.Length; i++)
        {
            CurrencyPresentationEntry entry = _currencyEntries[i];

            if (entry.Type == type)
            {
                displayName = entry.DisplayName;
                icon = entry.Icon;
                return true;
            }
        }

        displayName = string.Empty;
        icon = null;
        return false;
    }

    public bool TryGetPlaceable(PlaceableType type, out string displayName, out Sprite icon)
    {
        if (_placeableEntries == null)
        {
            displayName = string.Empty;
            icon = null;
            return false;
        }

        for (int i = 0; i < _placeableEntries.Length; i++)
        {
            PlaceablePresentationEntry entry = _placeableEntries[i];

            if (entry.Type == type)
            {
                displayName = entry.DisplayName;
                icon = entry.Icon;
                return true;
            }
        }

        displayName = string.Empty;
        icon = null;
        return false;
    }

    public bool TryGetPhase(DefendPhase phase, out string displayName, out Sprite icon)
    {
        if (_phaseEntries == null)
        {
            displayName = string.Empty;
            icon = null;
            return false;
        }

        for (int i = 0; i < _phaseEntries.Length; i++)
        {
            PhasePresentationEntry entry = _phaseEntries[i];

            if (entry.Phase == phase)
            {
                displayName = entry.DisplayName;
                icon = entry.Icon;
                return true;
            }
        }

        displayName = string.Empty;
        icon = null;
        return false;
    }

    public bool TryGetEnemy(EnemyConfigBase enemyConfig, out string displayName, out Sprite icon)
    {
        if (enemyConfig == null || _enemyEntries == null)
        {
            displayName = string.Empty;
            icon = null;
            return false;
        }

        for (int i = 0; i < _enemyEntries.Length; i++)
        {
            EnemyPresentationEntry entry = _enemyEntries[i];

            if (entry.EnemyConfig == enemyConfig)
            {
                displayName = entry.DisplayName;
                icon = entry.Icon;
                return true;
            }
        }

        displayName = string.Empty;
        icon = null;
        return false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_currencyEntries == null)
        {
            _currencyEntries = Array.Empty<CurrencyPresentationEntry>();
        }

        if (_placeableEntries == null)
        {
            _placeableEntries = Array.Empty<PlaceablePresentationEntry>();
        }

        if (_phaseEntries == null)
        {
            _phaseEntries = Array.Empty<PhasePresentationEntry>();
        }

        if (_enemyEntries == null)
        {
            _enemyEntries = Array.Empty<EnemyPresentationEntry>();
        }
    }
#endif

    [Serializable]
    public struct CurrencyPresentationEntry
    {
        [SerializeField] private CurrencyType _type;
        [SerializeField] private string _displayName;
        [SerializeField] private Sprite _icon;

        public CurrencyType Type => _type;
        public string DisplayName => _displayName;
        public Sprite Icon => _icon;
    }

    [Serializable]
    public struct PlaceablePresentationEntry
    {
        [SerializeField] private PlaceableType _type;
        [SerializeField] private string _displayName;
        [SerializeField] private Sprite _icon;

        public PlaceableType Type => _type;
        public string DisplayName => _displayName;
        public Sprite Icon => _icon;
    }

    [Serializable]
    public struct PhasePresentationEntry
    {
        [SerializeField] private DefendPhase _phase;
        [SerializeField] private string _displayName;
        [SerializeField] private Sprite _icon;

        public DefendPhase Phase => _phase;
        public string DisplayName => _displayName;
        public Sprite Icon => _icon;
    }

    [Serializable]
    public struct EnemyPresentationEntry
    {
        [SerializeField] private EnemyConfigBase _enemyConfig;
        [SerializeField] private string _displayName;
        [SerializeField] private Sprite _icon;

        public EnemyConfigBase EnemyConfig => _enemyConfig;
        public string DisplayName => _displayName;
        public Sprite Icon => _icon;
    }
}
