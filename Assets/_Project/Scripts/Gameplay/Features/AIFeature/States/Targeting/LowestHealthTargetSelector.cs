using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using System.Collections.Generic;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Targeting
{
    public sealed class LowestHealthTargetSelector : ITargetSelector
    {
        private readonly Entity _owner;

        public LowestHealthTargetSelector(Entity owner)
        {
            _owner = owner;
        }

        public Entity SelectTargetFrom(IEnumerable<Entity> targets)
        {
            Entity best = null;
            float bestHp = float.MaxValue;

            foreach (Entity candidate in targets)
            {
                if (candidate == _owner)
                    continue;

                if (candidate.HasComponent<CurrentHealth>() == false)
                    continue;

                if (candidate.HasComponent<IsDead>() == false)
                    continue;

                if (candidate.HasComponent<TransformComponent>() == false)
                    continue;

                if (candidate.IsDead.Value)
                    continue;

                float hp = candidate.CurrentHealth.Value;

                if (hp < bestHp)
                {
                    bestHp = hp;
                    best = candidate;
                }
            }

            return best;
        }
    }
}
