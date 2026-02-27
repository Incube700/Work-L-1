namespace Assets._Project.Scripts.Gameplay.Features.AttackFeature
{
    public sealed class AttackSettings
    {
        public AttackSettings(float attackDuration, float attackDelay, float attackCooldown)
        {
            AttackDuration = attackDuration;
            AttackDelay = attackDelay;
            AttackCooldown = attackCooldown;
        }

        public float AttackDuration { get; }
        public float AttackDelay { get; }
        public float AttackCooldown { get; }
    }
}