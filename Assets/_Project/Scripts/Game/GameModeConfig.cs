using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Game Modes Config", fileName = "GameModesConfig")]
public sealed class GameModesConfig : ScriptableObject
{
    [SerializeField] private List<ModeConfig> modes;

    public string GetAvailableChars(GameMode mode)
    {
        for (int i = 0; i < modes.Count; i++)
        {
            if (modes[i].Mode == mode)
            {
                string chars = modes[i].AvailableChars;

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