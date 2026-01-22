using UnityEngine;

public sealed class ConfigService
{
    private const string GameModesPath = "Configs/GameModesConfig";

    private GameModesConfig gameModes;

    public GameModesConfig GameModes
    {
        get
        {
            if (gameModes == null)
            {
                gameModes = Resources.Load<GameModesConfig>(GameModesPath);

                if (gameModes == null)
                {
                    throw new MissingReferenceException($"GameModesConfig not found at Resources/{GameModesPath}");
                }
            }

            return gameModes;
        }
    }
}