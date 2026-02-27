using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Utilities.Conditions;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.AttackFeature
{
    public sealed class StartAttackRequest : IEntityComponent
    {
        public SimpleEvent Value;
    }

    public sealed class StartAttackEvent : IEntityComponent
    {
        public SimpleEvent Value;
    }

    public sealed class CanStartAttack : IEntityComponent
    {
        public ICompositeCondition Value;
    }

    public sealed class EndAttackEvent : IEntityComponent
    {
        public SimpleEvent Value;
    }

    public sealed class AttackProcessInitialTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public sealed class AttackProcessCurrentTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public sealed class InAttackProcess : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }

    public sealed class AttackDelayTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public sealed class AttackDelayEndEvent : IEntityComponent
    {
        public SimpleEvent Value;
    }

    public sealed class MustCancelAttack : IEntityComponent
    {
        public ICompositeCondition Value;
    }

    public sealed class AttackCanceledEvent : IEntityComponent
    {
        public SimpleEvent Value;
    }

    public sealed class AttackCooldownInitialTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public sealed class AttackCooldownCurrentTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public sealed class InAttackCooldown : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }
}