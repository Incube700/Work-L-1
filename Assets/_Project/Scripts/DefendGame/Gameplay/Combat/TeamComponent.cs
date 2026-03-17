using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;

public sealed class TeamComponent : IEntityComponent
{
    public TeamComponent(Team value)
    {
        Value = value;
    }

    public Team Value { get; }
}