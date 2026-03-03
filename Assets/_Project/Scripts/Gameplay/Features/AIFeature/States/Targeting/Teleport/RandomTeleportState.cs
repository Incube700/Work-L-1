using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.TeleportFeature;
using Assets._Project.Scripts.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Teleport
{
    public sealed class RandomTeleportState : State, IUpdatableState
    {
        private readonly Entity _entity;
        private readonly SimpleEvent<Vector3> _teleportRequest;

        public RandomTeleportState(Entity entity)
        {
            _entity = entity;
            _teleportRequest = entity.TeleportRequest;
        }

        public override void Enter()
        {
            base.Enter();
            _teleportRequest.Invoke(TeleportPositionCalculator.GetRandomPoint(_entity));
        }

        public void Update(float deltaTime)
        {
        }
    }
}
