using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.TeleportFeature;
using Assets._Project.Scripts.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Teleport
{
    public sealed class SmartTeleportState : State, IUpdatableState
    {
        private readonly Entity _entity;
        private readonly SimpleEvent<Vector3> _teleportRequest;

        public SmartTeleportState(Entity entity)
        {
            _entity = entity;
            _teleportRequest = entity.TeleportRequest;
        }

        public override void Enter()
        {
            base.Enter();

            Vector3 position = TeleportPositionCalculator.GetPointTowardsCurrentTarget(_entity);
            _teleportRequest.Invoke(position);
        }

        public void Update(float deltaTime)
        {
        }
    }
}
