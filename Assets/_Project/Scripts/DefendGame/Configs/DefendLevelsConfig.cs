using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Defend Game/Defend Levels Config", fileName = "DefendLevelsConfig")]
public sealed class DefendLevelsConfig : ScriptableObject
{
    [SerializeField] private List<DefendLevelConfig> _levels = new List<DefendLevelConfig>();

    public IReadOnlyList<DefendLevelConfig> Levels => _levels;

    public DefendLevelConfig GetRandom()
    {
        if (_levels == null || _levels.Count == 0)
        {
            throw new InvalidOperationException("DefendLevelsConfig has no levels.");
        }

        int index = UnityEngine.Random.Range(0, _levels.Count);
        return _levels[index];
    }
}