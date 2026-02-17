using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.EntitiesCore
{
    public partial class Entity
    {
        public Rigidbody Rigidbody => GetComponent<RigidbodyComponent>().Value;
        
        public Entity AddRigidbody(Rigidbody rb) => AddComponent(new RigidbodyComponent { Value = rb });
    }
}
