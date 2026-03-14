using System;
using UnityEngine;

public sealed class IntervalTimer
{
    private readonly float _intervalSeconds;

    private float _timeLeft;

    public IntervalTimer(float intervalSeconds)
    {
        if (intervalSeconds <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(intervalSeconds), "Interval must be > 0.");
        }

        _intervalSeconds = intervalSeconds;
        _timeLeft = 0f;
    }

    public float IntervalSeconds => _intervalSeconds;
    public float TimeLeft => _timeLeft;

    public void Reset()
    {
        _timeLeft = 0f;
    }

    public int Tick(float deltaTime)
    {
        _timeLeft -= Mathf.Max(0f, deltaTime);

        int ticksCount = 0;

        while (_timeLeft <= 0f)
        {
            ticksCount++;
            _timeLeft += _intervalSeconds;
        }

        return ticksCount;
    }
}