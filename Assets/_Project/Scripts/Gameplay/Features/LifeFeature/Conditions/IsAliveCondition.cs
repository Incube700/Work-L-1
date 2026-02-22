using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Conditions;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;

namespace Assets._Project.Scripts.Gameplay.Features.LifeFeature.Conditions
{
    public sealed class IsAliveCondition : ICondition
    {
        public bool IsSatisfied(Entity entity)
        {
            if (entity.TryGetComponent(out IsDead isDead) == false)
                return true;

            return isDead.Value.Value == false;
        }
    }
}