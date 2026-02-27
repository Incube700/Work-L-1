using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Utilities.StateMachineCore;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Teleport
{
    public sealed class SmartTeleportState : State, IUpdatableState
    {
        private readonly Entity _entity;
        private readonly float _interval;
        private readonly float _minEnergyPercent;

        private float _timer;

        private readonly SimpleEvent _teleportRequest;

        private readonly ReactiveVariable<float> _currentEnergy;
        private readonly ReactiveVariable<float> _maxEnergy;
        private readonly float _energyCost;

        private readonly ReactiveVariable<Entity> _currentTarget;

        public SmartTeleportState(Entity entity, float interval, float minEnergyPercent)
        {
            _entity = entity;
            _interval = interval;
            _minEnergyPercent = minEnergyPercent;

            _teleportRequest = entity.TeleportRequest;

            _currentEnergy = entity.CurrentEnergy;
            _maxEnergy = entity.MaxEnergy;
            _energyCost = entity.TeleportEnergyCost;

            _currentTarget = entity.GetComponent<CurrentTarget>().Value;

            _timer = 0f;
        }

        public void Update(float deltaTime)
        {
            _timer += deltaTime;

            if (_timer > _interval)
                _timer = _interval;

            var max = _maxEnergy.Value;

            if (max <= 0f)
                return;

            var percent = _currentEnergy.Value / max;

            if (percent < _minEnergyPercent)
                return;

            if (_timer < _interval)
                return;

            if (_currentTarget.Value == null)
                return;

            if (_currentEnergy.Value < _energyCost)
                return;

            _timer = 0f;
            _teleportRequest.Invoke();
        }
    }
}