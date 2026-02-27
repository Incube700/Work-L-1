using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Conditions;
using Assets._Project.Scripts.Gameplay.Features.EnergyFeature;

namespace Assets._Project.Scripts.Gameplay.Features.TeleportFeature.Conditions
{
    public sealed class HasEnoughEnergyForTeleportCondition : ICondition
    {
        public bool IsSatisfied(Entity entity)
        {
            CurrentEnergy energy = entity.GetComponent<CurrentEnergy>();
            TeleportEnergyCost cost = entity.GetComponent<TeleportEnergyCost>();

            return energy.Value.Value >= cost.Value;
        }
    }
}