using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.TeleportFeature;
using Assets._Project.Scripts.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Teleport
{
    public sealed class TeleportState : State, IUpdatableState
    {
        private readonly Entity _entity;
        private readonly ITeleportPositionCalculator _calculator;
        private readonly SimpleEvent<Vector3> _teleportRequest;

        public TeleportState(Entity entity, ITeleportPositionCalculator calculator)
        {
            _entity = entity;
            _calculator = calculator;
            _teleportRequest = entity.TeleportRequest;
        }

        public override void Enter()
        {
            base.Enter();
            Vector3 position = _calculator.Calculate(_entity);
            _teleportRequest.Invoke(position);
        }

        public void Update(float deltaTime)
        {
        }
    }
}
