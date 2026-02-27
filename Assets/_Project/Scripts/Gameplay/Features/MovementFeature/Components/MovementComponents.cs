using Assets._Project.Scripts.Gameplay.EntitiesCore;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.MovementFeature
{
    public class MoveDirection : IEntityComponent
    {
        public ReactiveVariable<Vector3> Value;
    }

    public class MoveSpeed : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TransformComponent : IEntityComponent
    {
        public Transform Value;
    }
    
    public class RotationDirection : IEntityComponent
    {
        public ReactiveVariable<Vector3> Value;
    }
}
