using System;
using System.Collections.Generic;
using Assets._Project.Scripts.Utilities.StateMachineCore;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature
{
    public sealed class AIStateMachine : StateMachine<IUpdatableState>
    {
        public AIStateMachine(List<IDisposable> disposables) : base(disposables) { }

        public AIStateMachine() : base(new List<IDisposable>()) { }

        protected override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
            CurrentState?.Update(deltaTime);
        }
    }
}