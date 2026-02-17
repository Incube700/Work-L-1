using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.MovementFeature
{
    public class RigidbodyEntityRegistrator : MonoEntityRegistrator
    {
        public override void Register(Entity entity)
        {
            entity.AddRigidbody(GetComponent<Rigidbody>());
        }
    }
}
