using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Conditions;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;

namespace Assets._Project.Scripts.Gameplay.Features.EnergyFeature
{
    public sealed class EnergyRegenerationSystem : IInitializableSystem, IUpdatableSystem
    {
        private readonly ICondition _condition;

        private Entity _entity;

        private ReactiveVariable<float> _maxEnergy;
        private ReactiveVariable<float> _currentEnergy;

        private float _regenInterval;
        private float _regenPercent;

        private EnergyRegenTimer _timerComponent;

        public EnergyRegenerationSystem(ICondition condition)
        {
            _condition = condition;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;

            _maxEnergy = entity.MaxEnergy;
            _currentEnergy = entity.CurrentEnergy;
            _regenInterval = entity.EnergyRegenInterval;
            _regenPercent = entity.EnergyRegenPercent;

            _timerComponent = entity.EnergyRegenTimerComponent;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_condition.IsSatisfied(_entity) == false)
                return;

            if (_regenInterval <= 0f)
                return;

            _timerComponent.Value += deltaTime;

            if (_timerComponent.Value < _regenInterval)
                return;

            _timerComponent.Value -= _regenInterval;

            float add = _maxEnergy.Value * _regenPercent;
            float next = _currentEnergy.Value + add;

            if (next > _maxEnergy.Value)
                next = _maxEnergy.Value;

            _currentEnergy.Value = next;
        }
    }
}