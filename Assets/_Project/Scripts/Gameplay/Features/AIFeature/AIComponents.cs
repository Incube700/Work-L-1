using Assets._Project.Scripts.Gameplay.EntitiesCore;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature
{
    public sealed class CurrentTarget : IEntityComponent
    {
        public ReactiveVariable<Entity> Value;
    }
}