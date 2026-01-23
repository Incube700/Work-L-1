using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Configs/Game Modes Config", fileName = "GameModesConfig")]
public sealed class GameModesConfig : ScriptableObject
{
    [FormerlySerializedAs("modes")] [SerializeField] private List<ModeConfig> _modes;

    public string GetAvailableChars(GameMode mode)
    {
        for (int i = 0; i < _modes.Count; i++)
        {
            if (_modes[i].Mode == mode)
            {
                string chars = _modes[i].AvailableChars;

                if (string.IsNullOrEmpty(chars))
                {
                    throw new InvalidOperationException($"AvailableChars is empty for mode: {mode}");
                }

                return chars;
            }
        }

        throw new InvalidOperationException($"Config not found for mode: {mode}");
    }

    [Serializable]
    private class ModeConfig
    {
        [field: SerializeField] public GameMode Mode { get; private set; }
        [field: SerializeField] public string AvailableChars { get; private set; }
    }
}