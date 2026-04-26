using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;

public static class CombatTargetFilter
{
    public static bool CanDamage(Entity attacker, Entity target)
    {
        if (attacker == null || target == null)
        {
            return false;
        }

        if (attacker == target)
        {
            return false;
        }

        if (attacker.TryGetComponent(out TeamComponent attackerTeam) == false)
        {
            return false;
        }

        if (target.TryGetComponent(out TeamComponent targetTeam) == false)
        {
            return false;
        }

        if (targetTeam.Value == attackerTeam.Value)
        {
            return false;
        }

        if (target.TryGetComponent(out IsDead isDead))
        {
            if (isDead.Value.Value == true)
            {
                return false;
            }
        }

        return target.HasComponent<TakeDamageRequest>();
    }

    public static bool CanDamageFromTeam(Team attackerTeamValue, Entity target)
    {
        if (target == null)
        {
            return false;
        }

        if (target.TryGetComponent(out TeamComponent targetTeam) == false)
        {
            return false;
        }

        if (targetTeam.Value == attackerTeamValue)
        {
            return false;
        }

        if (target.TryGetComponent(out IsDead isDead))
        {
            if (isDead.Value.Value == true)
            {
                return false;
            }
        }

        return target.HasComponent<TakeDamageRequest>();
    }
}