using Assets._Project.Scripts.Gameplay.EntitiesCore;
using System.Collections.Generic;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Targeting
{
    public interface ITargetSelector
    {
        Entity SelectTargetFrom(IEnumerable<Entity> targets);
    }
}