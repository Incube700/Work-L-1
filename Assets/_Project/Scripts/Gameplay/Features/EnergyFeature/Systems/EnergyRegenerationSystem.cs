using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;

namespace Assets._Project.Scripts.Gameplay.Features.EnergyFeature
{
    public sealed class EnergyRegenerationSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _maxEnergy;
        private ReactiveVariable<float> _currentEnergy;
        private float _regenInterval;
        private float _regenPercent;

        private EnergyRegenTimer _timerComponent;
        private ReactiveVariable<bool> _isDead;

        public void OnInit(Entity entity)
        {
            _maxEnergy = entity.GetComponent<MaxEnergy>().Value;
            _currentEnergy = entity.GetComponent<CurrentEnergy>().Value;
            _regenInterval = entity.GetComponent<EnergyRegenInterval>().Value;
            _regenPercent = entity.GetComponent<EnergyRegenPercent>().Value;
            _timerComponent = entity.GetComponent<EnergyRegenTimer>();

            // Энергия восстанавливается только у живых.
            _isDead = entity.GetComponent<IsDead>().Value;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isDead.Value)
                return;

            if (_regenInterval <= 0f)
                return;

            // Копим время, пока не пройдёт Z секунд.
            _timerComponent.Value += deltaTime;

            if (_timerComponent.Value < _regenInterval)
                return;

            // "Тик" регена: раз в Z секунд добавляем 10% (или другой процент) от MaxEnergy.
            _timerComponent.Value -= _regenInterval;

            float add = _maxEnergy.Value * _regenPercent;
            if (add <= 0f)
                return;

            float next = _currentEnergy.Value + add;
            if (next > _maxEnergy.Value)
                next = _maxEnergy.Value;

            _currentEnergy.Value = next;
        }
    }
}
