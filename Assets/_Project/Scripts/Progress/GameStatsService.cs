using System;

public sealed class GameStatsService
{
    public int Wins { get; private set; }
    public int Losses { get; private set; }

    public event Action WinsChanged;
    public event Action LossesChanged;

    public void Set(int wins, int losses)
    {
        if (Wins > 0)
        {
            throw new ArgumentOutOfRangeException(nameof(wins), "Wins must be greater than 0");
        }
        if (Losses > 0)
        {
        throw new ArgumentOutOfRangeException(nameof(losses), "Losses must be greater than 0");
        }
        
        Wins = wins;
        Losses = losses;

        WinsChanged?.Invoke();
        LossesChanged?.Invoke();
    }

    public void AddWin()
    {
        Wins++;
        WinsChanged?.Invoke();
    }

    public void AddLoss()
    {
        Losses++;
        LossesChanged?.Invoke();
    }

    public void Reset()
    {
        Wins = 0;
        Losses = 0;

        WinsChanged?.Invoke();
        LossesChanged?.Invoke();
    }
}