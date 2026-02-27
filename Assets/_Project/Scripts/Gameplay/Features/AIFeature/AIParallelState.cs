using Assets._Project.Scripts.Utilities.StateMachineCore;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature
{
    public sealed class AIParallelState : ParallelState<IUpdatableState>, IUpdatableState
    {
        public AIParallelState(params IUpdatableState[] states) : base(states) { }

        public void Update(float deltaTime)
        {
            foreach (IUpdatableState state in States)
                state.Update(deltaTime);
        }
    }
}