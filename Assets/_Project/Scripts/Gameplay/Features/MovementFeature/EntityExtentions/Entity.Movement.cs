using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.EntitiesCore
{
    public partial class Entity
    {
        public ReactiveVariable<Vector3> MoveDirection => GetComponent<MoveDirection>().Value;
        public ReactiveVariable<float> MoveSpeed => GetComponent<MoveSpeed>().Value;
        
        public Entity AddMoveDirection() => AddComponent(new MoveDirection { Value = new ReactiveVariable<Vector3>(Vector3.zero) });
        public Entity AddMoveSpeed(ReactiveVariable<float> value) => AddComponent(new MoveSpeed { Value = value });
    }
}
