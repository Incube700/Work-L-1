using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Conditions;

namespace Assets._Project.Scripts.Gameplay.Features.TeleportFeature
{
    public sealed class CanTeleportConditionComponent : IEntityComponent
    {
        public ICondition Value;
    }
}
