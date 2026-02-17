using UnityEngine;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;

namespace Assets._Project.Scripts.Gameplay.Features.MovementFeature
{
    public sealed class CharacterControllerEntityRegistrator : MonoEntityRegistrator
    {
        public override void Register(Entity entity)
        {
            UnityEngine.CharacterController controller = GetComponent<UnityEngine.CharacterController>();
            entity.AddComponent(new CharacterControllerComponent { Value = controller });
            entity.AddComponent(new TransformComponent { Value = transform });
        }
    }
}