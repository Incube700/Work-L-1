using Assets._Project.Scripts.Gameplay.EntitiesCore;

namespace Assets._Project.Scripts.Gameplay.Features.TeleportFeature
{
    public sealed class TeleportRadius : IEntityComponent
    {
        public float Value;
    }

    public sealed class TeleportEnergyCost : IEntityComponent
    {
        public float Value;
    }

    public sealed class TeleportRequest : IEntityComponent
    {
        public SimpleEvent Value;
    }

    public sealed class TeleportedEvent : IEntityComponent
    {
        public SimpleEvent Value;
    }

    public sealed class TeleportAoEDamage : IEntityComponent
    {
        public float Value;
    }

    public sealed class TeleportAoERadius : IEntityComponent
    {
        public float Value;
    }

    public sealed class TeleportAoEMask : IEntityComponent
    {
        public int Value;
    }
}
