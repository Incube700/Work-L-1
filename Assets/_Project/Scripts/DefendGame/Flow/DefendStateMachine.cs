using System;
using System.Collections.Generic;
using Assets._Project.Scripts.Utilities.StateMachineCore;

public sealed class DefendStateMachine : StateMachine<IState>
{
    public DefendStateMachine() : base(new List<IDisposable>())
    {
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (CurrentState is IUpdatableState updatableState)
        {
            updatableState.Update(deltaTime);
        }
    }
}