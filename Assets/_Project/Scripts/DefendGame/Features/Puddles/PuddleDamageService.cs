using System.Collections.Generic;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;

public sealed class PuddleDamageService
{
    private readonly float _damagePerTick;

    public PuddleDamageService(float damagePerTick)
    {
        _damagePerTick = damagePerTick;
    }

    public int Apply(List<Entity> targets)
    {
        if (targets == null)
        {
            return 0;
        }

        int damagedCount = 0;

        for (int i = 0; i < targets.Count; i++)
        {
            Entity target = targets[i];

            if (target == null)
            {
                continue;
            }

            if (target.HasComponent<TakeDamageRequest>() == false)
            {
                continue;
            }

            if (target.TryGetComponent(out IsDead isDead))
            {
                if (isDead.Value.Value == true)
                {
                    continue;
                }
            }

            target.TakeDamageRequest.Invoke(_damagePerTick);
            damagedCount++;
        }

        return damagedCount;
    }
}