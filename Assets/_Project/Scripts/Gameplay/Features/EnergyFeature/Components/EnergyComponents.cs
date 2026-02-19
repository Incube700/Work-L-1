using Assets._Project.Scripts.Gameplay.EntitiesCore;

namespace Assets._Project.Scripts.Gameplay.Features.EnergyFeature
{
    public sealed class MaxEnergy : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public sealed class CurrentEnergy : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public sealed class EnergyRegenInterval : IEntityComponent
    {
        public float Value;
    }

    public sealed class EnergyRegenPercent : IEntityComponent
    {
        public float Value;
    }

    public sealed class EnergyRegenTimer : IEntityComponent
    {
        public float Value;
    }
}
