using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "Configs/Permanent Upgrades Config",
    fileName = "PermanentUpgradesConfig")]
public sealed class PermanentUpgradesConfig : ScriptableObject
{
    [SerializeField] private List<PermanentUpgradeConfigBase> _upgrades =
        new List<PermanentUpgradeConfigBase>();

    public IReadOnlyList<PermanentUpgradeConfigBase> Upgrades => _upgrades;

    public PermanentUpgradeConfigBase GetUpgrade(PermanentUpgradeType type)
    {
        for (int i = 0; i < _upgrades.Count; i++)
        {
            PermanentUpgradeConfigBase config = _upgrades[i];

            if (config == null)
            {
                continue;
            }

            if (config.Type == type)
            {
                return config;
            }
        }

        throw new ArgumentOutOfRangeException(
            nameof(type),
            type,
            $"Permanent upgrade config is missing for type: {type}");
    }
}