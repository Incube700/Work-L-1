using System.Collections.Generic;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using UnityEngine;

public sealed class AreaAttackRadius : IEntityComponent
{
    public float Value;
}

public sealed class AreaAttackDamage : IEntityComponent
{
    public float Value;
}

public sealed class AreaAttackMask : IEntityComponent
{
    public int Value;
}

public sealed class AreaAttackRequest : IEntityComponent
{
    public SimpleEvent<Vector3> Value;
}

public sealed class AreaAttackTargets : IEntityComponent
{
    public List<Entity> Value;
}

public sealed class AreaAttackTargetsCollected : IEntityComponent
{
    public SimpleEvent<Vector3> Value;
}

public sealed class AreaAttackFinished : IEntityComponent
{
    public SimpleEvent Value;
}

public sealed class AreaAttackShouldShowExplosion : IEntityComponent
{
    public bool Value;
}

public sealed class OverlapActivationRadius : IEntityComponent
{
    public float Value;
}

public sealed class OverlapActivationMask : IEntityComponent
{
    public int Value;
}

public sealed class TimedAreaAttackInterval : IEntityComponent
{
    public float Value;
}

public sealed class TimedAreaAttackLeft : IEntityComponent
{
    public ReactiveVariable<float> Value;
}