using Assets._Project.Scripts.Gameplay.EntitiesCore;

namespace Assets._Project.Scripts.Gameplay.Features.LifeFeature
{
    public sealed class MaxHealth : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public sealed class CurrentHealth : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public sealed class IsDead : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }

    public sealed class TakeDamageRequest : IEntityComponent
    {
        public SimpleEvent<float> Value;
    }
}