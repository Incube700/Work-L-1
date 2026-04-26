using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using UnityEngine;

public sealed class BuildingCombatProjectileRequestSystem : IInitializableSystem, IDisposableSystem
{
    private readonly BuildingCombatService _buildingCombatService;

    private SimpleEvent<Vector3> _shootRequest;

    public BuildingCombatProjectileRequestSystem(BuildingCombatService buildingCombatService)
    {
        _buildingCombatService = buildingCombatService ??
                                 throw new ArgumentNullException(nameof(buildingCombatService));
    }

    public void OnInit(Entity entity)
    {
        _shootRequest = entity.GetComponent<ProjectileShootRequest>().Value;
        _buildingCombatService.AttackPerformed += OnAttackPerformed;
    }

    public void OnDispose()
    {
        _buildingCombatService.AttackPerformed -= OnAttackPerformed;
    }

    private void OnAttackPerformed(Vector3 targetPoint)
    {
        _shootRequest.Invoke(targetPoint);
    }
    
    public sealed class ProjectileShootDamageMultiplier : IEntityComponent
    {
        public float Value;
    }
}