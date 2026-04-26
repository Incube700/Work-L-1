using Assets._Project.Scripts.Gameplay.EntitiesCore;
using UnityEngine;

public sealed class ProjectileHitDistance : IEntityComponent
{
    public float Value;
}

public sealed class ProjectileLifetime : IEntityComponent
{
    public ReactiveVariable<float> Value;
}

public sealed class ProjectileTargetPoint : IEntityComponent
{
    public Vector3 Value;
}