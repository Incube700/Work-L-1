using Assets._Project.Scripts.Utilities.Conditions;

public sealed class DefendStateMachineFactory
{
    private readonly DefendPhaseService _phaseService;
    private readonly RestTimerService _restTimerService;
    private readonly WaveProgressService _waveProgressService;
    private readonly BuildingStateService _buildingStateService;
    private readonly DefendResultService _resultService;
    private readonly EnemySpawner _enemySpawner;
    private readonly EnemyService _enemyService;
    private readonly float _restDurationSeconds;

    public DefendStateMachineFactory(
        DefendPhaseService phaseService,
        RestTimerService restTimerService,
        WaveProgressService waveProgressService,
        BuildingStateService buildingStateService,
        DefendResultService resultService,
        EnemySpawner enemySpawner,
        EnemyService enemyService,
        float restDurationSeconds)
    {
        _phaseService = phaseService;
        _restTimerService = restTimerService;
        _waveProgressService = waveProgressService;
        _buildingStateService = buildingStateService;
        _resultService = resultService;
        _enemySpawner = enemySpawner;
        _enemyService = enemyService;
        _restDurationSeconds = restDurationSeconds;
    }

    public DefendStateMachine Create()
    {
        DefendStateMachine stateMachine = new DefendStateMachine();

        DefendWaveState waveState = new DefendWaveState(
            _phaseService,
            _waveProgressService,
            _enemySpawner,
            _enemyService);

        DefendRestState restState = new DefendRestState(
            _phaseService,
            _restTimerService,
            _restDurationSeconds);

        DefendWinState winState = new DefendWinState(_resultService);
        DefendLoseState loseState = new DefendLoseState(_resultService);

        stateMachine.AddState(waveState);
        stateMachine.AddState(restState);
        stateMachine.AddState(winState);
        stateMachine.AddState(loseState);

        ICondition buildingDead = new FuncCondition(() => _buildingStateService.IsDead);
        ICondition waveCompleted = new FuncCondition(() => waveState.IsCompleted);
        ICondition hasNextWave = new FuncCondition(() => _waveProgressService.HasNextWave);
        ICondition noNextWave = new FuncCondition(() => _waveProgressService.HasNextWave == false);
        ICondition restFinished = new FuncCondition(() => restState.IsFinished);

        stateMachine.AddTransition(waveState, loseState, buildingDead);
        stateMachine.AddTransition(restState, loseState, buildingDead);

        stateMachine.AddTransition(
            waveState,
            restState,
            new CompositeCondition()
                .Add(waveCompleted)
                .Add(hasNextWave));

        stateMachine.AddTransition(
            waveState,
            winState,
            new CompositeCondition()
                .Add(waveCompleted)
                .Add(noNextWave));

        stateMachine.AddTransition(
            restState,
            waveState,
            new CompositeCondition()
                .Add(restFinished)
                .Add(hasNextWave));

        return stateMachine;
    }
}