using Assets._Project.Scripts.Gameplay.EntitiesCore;
using UnityEngine;

public sealed class ProjectileShootRequest : IEntityComponent
{
    public SimpleEvent<Vector3> Value;
}

public sealed class ProjectileShootInterval : IEntityComponent
{
    public float Value;
}

public sealed class ProjectileShootCooldown : IEntityComponent
{
    public ReactiveVariable<float> Value;
}

public sealed class ProjectileShootConfig : IEntityComponent
{
    public ProjectileConfig Value;
}