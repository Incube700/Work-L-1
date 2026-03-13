using System;
using Assets._Project.Scripts.Utilities.StateMachineCore;

public sealed class DefendWaveState : State, IUpdatableState
{
    private readonly DefendGameController _controller;

    private bool _spawnCompletedLogged;
    private bool _waveClearedLogged;

    public DefendWaveState(DefendGameController controller)
    {
        _controller = controller;
    }

    public bool IsCompleted => _controller.IsWaveSpawnCompleted && _controller.AliveEnemiesCount == 0;

    public override void Enter()
    {
        base.Enter();

        _controller.SetPhase(DefendPhase.Wave);

        int waveIndex = _controller.MoveToNextWave();
        WaveConfig wave = _controller.GetWaveConfig(waveIndex);

        _controller.StartWaveSpawn(wave);

        _spawnCompletedLogged = false;
        _waveClearedLogged = false;

        _controller.LogState(
            $"[Defend] Wave {waveIndex + 1}/{_controller.WavesCount} started. Enemies: {wave.EnemiesCount}, SpawnInterval: {wave.SpawnInterval:0.##}s");
    }

    public void Update(float deltaTime)
    {
        _controller.UpdateWaveSpawn(deltaTime);

        if (_controller.IsWaveSpawnCompleted && _spawnCompletedLogged == false)
        {
            _spawnCompletedLogged = true;
            _controller.LogState($"[Defend] Wave {_controller.CurrentWaveNumber}: all enemies spawned.");
        }

        if (_controller.IsWaveSpawnCompleted && _controller.AliveEnemiesCount == 0 && _waveClearedLogged == false)
        {
            _waveClearedLogged = true;
            _controller.LogState($"[Defend] Wave {_controller.CurrentWaveNumber} cleared.");
        }
    }
}