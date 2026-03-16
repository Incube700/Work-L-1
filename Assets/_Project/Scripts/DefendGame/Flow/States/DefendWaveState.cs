using Assets._Project.Scripts.Utilities.StateMachineCore;
using UnityEngine;

public sealed class DefendWaveState : State, IUpdatableState
{
    private readonly DefendPhaseService _phaseService;
    private readonly WaveProgressService _waveProgressService;
    private readonly EnemySpawner _enemySpawner;
    private readonly EnemyService _enemyService;

    private bool _spawnCompletedLogged;
    private bool _waveClearedLogged;

    public DefendWaveState(
        DefendPhaseService phaseService,
        WaveProgressService waveProgressService,
        EnemySpawner enemySpawner,
        EnemyService enemyService)
    {
        _phaseService = phaseService;
        _waveProgressService = waveProgressService;
        _enemySpawner = enemySpawner;
        _enemyService = enemyService;
    }

    public bool IsCompleted => _enemySpawner.IsCompleted && _enemyService.AliveCount == 0;

    public override void Enter()
    {
        base.Enter();

        _phaseService.SetPhase(DefendPhase.Wave);

        int waveIndex = _waveProgressService.MoveToNextWave();
        WaveConfig wave = _waveProgressService.GetWaveConfig(waveIndex);

        _enemySpawner.StartWave(wave);

        _spawnCompletedLogged = false;
        _waveClearedLogged = false;

        Log(
            $"[Defend] Wave {waveIndex + 1}/{_waveProgressService.WavesCount} started. Enemies: {wave.EnemiesCount}, SpawnInterval: {wave.SpawnInterval:0.##}s");
    }

    public void Update(float deltaTime)
    {
        _enemySpawner.Update(deltaTime);

        if (_enemySpawner.IsCompleted && _spawnCompletedLogged == false)
        {
            _spawnCompletedLogged = true;
            Log($"[Defend] Wave {_waveProgressService.CurrentWaveNumber}: all enemies spawned.");
        }

        if (_enemySpawner.IsCompleted && _enemyService.AliveCount == 0 && _waveClearedLogged == false)
        {
            _waveClearedLogged = true;
            Log($"[Defend] Wave {_waveProgressService.CurrentWaveNumber} cleared.");
        }
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        Debug.Log(message);
    }
}