using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Conditions;

namespace Assets._Project.Scripts.Gameplay.Features.TeleportFeature.Conditions
{
    public sealed class HasEnoughEnergyForTeleportCondition : ICondition
    {
        public bool IsSatisfied(Entity entity)
        {
            return entity.CurrentEnergy.Value >= entity.TeleportEnergyCost;
        }
    }
}
