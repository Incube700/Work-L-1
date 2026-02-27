using Assets._Project.Scripts.Gameplay.Features.InputFeature;

namespace Assets._Project.Scripts.Gameplay.Features.AttackFeature
{
    public sealed class AttackInputAdapter : IAttackInput
    {
        private readonly IInputService _input;

        public AttackInputAdapter(IInputService input)
        {
            _input = input;
        }

        public float MoveSqrMagnitude => _input.Direction.sqrMagnitude;
    }
}
