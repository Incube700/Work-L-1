using System;

public sealed class WaveProgressService
{
    private readonly DefendLevelConfig _level;

    private int _currentWaveIndex = -1;

    public WaveProgressService(DefendLevelConfig level)
    {
        _level = level ?? throw new ArgumentNullException(nameof(level));
    }

    public int CurrentWaveIndex => _currentWaveIndex;
    public int CurrentWaveNumber => _currentWaveIndex + 1;
    public int WavesCount => _level.Waves.Count;
    public bool HasAnyWaves => _level.Waves.Count > 0;
    public bool HasNextWave => _currentWaveIndex + 1 < _level.Waves.Count;

    public int MoveToNextWave()
    {
        if (HasNextWave == false)
        {
            throw new InvalidOperationException("No next wave available.");
        }

        _currentWaveIndex++;
        return _currentWaveIndex;
    }

    public WaveConfig GetWaveConfig(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= _level.Waves.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(waveIndex));
        }

        return _level.Waves[waveIndex];
    }
}