using UnityEngine;

public sealed class MenuFlow
{
    private const char NumbersKey = '1';
    private const char LettersKey = '2';

    private readonly KeyboardInputReader _input;
    private readonly SceneLoader _sceneLoader;

    private readonly GameStatsService _stats;
    private readonly WalletService _wallet;
    private readonly ProgressResetService _reset;
    private readonly SaveService _save;

    public MenuFlow(
        KeyboardInputReader input,
        SceneLoader sceneLoader,
        GameStatsService stats,
        WalletService wallet,
        ProgressResetService reset,
        SaveService save)
    {
        _input = input;
        _sceneLoader = sceneLoader;
        _stats = stats;
        _wallet = wallet;
        _reset = reset;
        _save = save;
    }

    public void Start()
    {
        Debug.Log("MAIN MENU: 1 - Numbers, 2 - Letters");
        Debug.Log("F1 - Stats, F2 - Reset stats (paid), F3 - Wipe save to defaults");

        _input.CharTyped += OnCharTyped;
        _input.StatsPressed += OnStatsPressed;
        _input.ResetPressed += OnResetPressed;
        _input.WipePressed += OnWipePressed;
    }

    public void Stop()
    {
        _input.CharTyped -= OnCharTyped;
        _input.StatsPressed -= OnStatsPressed;
        _input.ResetPressed -= OnResetPressed;
        _input.WipePressed -= OnWipePressed;
    }

    private void OnCharTyped(char c)
    {
        if (c == NumbersKey)
        {
            _sceneLoader.Load(SceneNames.Gameplay, new GameplayArgs(GameMode.Numbers));
        }
        else if (c == LettersKey)
        {
            _sceneLoader.Load(SceneNames.Gameplay, new GameplayArgs(GameMode.Letters));
        }
    }

    private void OnStatsPressed()
    {
        Debug.Log($"STATS: Wins={_stats.WinsValue}, Losses={_stats.LossesValue}, Gold={_wallet.GoldValue}");
    }

    private void OnResetPressed()
    {
        if (_reset.TryResetStats(out string failReason))
        {
            Debug.Log($"Stats reset OK. Gold={_wallet.GoldValue}");
        }
        else
        {
            Debug.Log(failReason);
        }
    }

    private void OnWipePressed()
    {
        _save.DeleteAll();
        _save.LoadAll(); // заново выставит дефолты (0/0 и StartGold) и сохранит

        Debug.Log($"SAVE WIPED. Wins={_stats.WinsValue}, Losses={_stats.LossesValue}, Gold={_wallet.GoldValue}");
    }
}
