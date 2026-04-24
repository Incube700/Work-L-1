using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class PermanentUpgradeDefinition
{
    [SerializeField] private PermanentUpgradeType _type;
    [SerializeField] private string _title = string.Empty;
    [SerializeField, TextArea] private string _description = string.Empty;
    [SerializeField, Min(1)] private int _costDiamonds = 1;

    public PermanentUpgradeType Type => _type;
    public string Title => _title;
    public string Description => _description;
    public int CostDiamonds => _costDiamonds;
}

[CreateAssetMenu(menuName = "Configs/Permanent Upgrades Config", fileName = "PermanentUpgradesConfig")]
public sealed class PermanentUpgradesConfig : ScriptableObject
{
    [SerializeField] private List<PermanentUpgradeDefinition> _definitions =
        new List<PermanentUpgradeDefinition>();

    [Header("Wave Heal")]
    [SerializeField, Min(0f)] private float _waveHealPercent = 10f;

    [Header("Opening Strike")]
    [SerializeField, Min(0f)] private float _openingStrikeDamagePercent = 35f;
    [SerializeField, Min(0)] private int _openingStrikeTargetsCount = 3;

    [Header("Player Explosion Damage")]
    [SerializeField, Min(0f)] private float _playerExplosionDamagePercent = 25f;

    public float WaveHealPercent => _waveHealPercent;
    public float OpeningStrikeDamagePercent => _openingStrikeDamagePercent;
    public int OpeningStrikeTargetsCount => _openingStrikeTargetsCount;
    public float PlayerExplosionDamagePercent => _playerExplosionDamagePercent;

    public PermanentUpgradeDefinition GetDefinition(PermanentUpgradeType type)
    {
        for (int i = 0; i < _definitions.Count; i++)
        {
            PermanentUpgradeDefinition definition = _definitions[i];

            if (definition.Type == type)
            {
                return definition;
            }
        }

        throw new ArgumentOutOfRangeException(
            nameof(type),
            type,
            $"Permanent upgrade definition is missing for type: {type}");
    }
}