using Assets._Project.Scripts.Utilities.Conditions;
using Assets._Project.Scripts.Utilities.StateMachineCore;

public sealed class DefendStateMachineFactory
{
    public DefendStateMachine Create(DefendGameController controller)
    {
        DefendStateMachine stateMachine = new DefendStateMachine();

        DefendWaveState waveState = new DefendWaveState(controller);
        DefendRestState restState = new DefendRestState(controller);
        DefendWinState winState = new DefendWinState(controller);
        DefendLoseState loseState = new DefendLoseState(controller);

        stateMachine.AddState(waveState);
        stateMachine.AddState(restState);
        stateMachine.AddState(winState);
        stateMachine.AddState(loseState);

        ICondition buildingDead = new FuncCondition(() => controller.IsBuildingDead);
        ICondition waveCompleted = new FuncCondition(() => waveState.IsCompleted);
        ICondition hasNextWave = new FuncCondition(() => controller.HasNextWave);
        ICondition noNextWave = new FuncCondition(() => controller.HasNextWave == false);
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