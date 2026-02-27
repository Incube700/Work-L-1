using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.EnergyFeature;
using Assets._Project.Scripts.Utilities.StateMachineCore;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Teleport
{
    public sealed class RandomTeleportState : State, IUpdatableState
    {
        private readonly Entity _entity;
        private readonly float _interval;

        private float _timer;

        private SimpleEvent _teleportRequest;

        private ReactiveVariable<float> _currentEnergy;
        private float _energyCost;

        public RandomTeleportState(Entity entity, float interval)
        {
            _entity = entity;
            _interval = interval;

            _teleportRequest = entity.TeleportRequest;

            _currentEnergy = entity.CurrentEnergy;
            _energyCost = entity.TeleportEnergyCost;

            _timer = 0f;
        }

        public void Update(float deltaTime)
        {
            _timer += deltaTime;

            if (_timer < _interval)
                return;

            if (_timer > _interval)
                _timer = _interval;

            if (_currentEnergy.Value < _energyCost)
                return;

            _timer = 0f;
            _teleportRequest.Invoke();
        }
    }
}