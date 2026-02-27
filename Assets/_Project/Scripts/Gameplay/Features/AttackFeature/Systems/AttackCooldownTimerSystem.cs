using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using System;

namespace Assets._Project.Scripts.Gameplay.Features.AttackFeature
{
    public sealed class AttackCooldownTimerSystem : IInitializableSystem, IDisposableSystem, IUpdatableSystem
    {
        private ReactiveVariable<bool> _inCooldown;
        private ReactiveVariable<float> _currentCooldown;
        private ReactiveVariable<float> _initialCooldown;

        private SimpleEvent _endAttackEvent;
        private IDisposable _endSubscription;

        public void OnInit(Entity entity)
        {
            _inCooldown = entity.GetComponent<InAttackCooldown>().Value;
            _currentCooldown = entity.GetComponent<AttackCooldownCurrentTime>().Value;
            _initialCooldown = entity.GetComponent<AttackCooldownInitialTime>().Value;

            _endAttackEvent = entity.GetComponent<EndAttackEvent>().Value;
            _endSubscription = _endAttackEvent.Subscribe(OnEndAttack);
        }

        public void OnDispose()
        {
            _endSubscription?.Dispose();
            _endSubscription = null;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inCooldown.Value == false)
                return;

            _currentCooldown.Value -= deltaTime;

            if (_currentCooldown.Value > 0f)
                return;

            _currentCooldown.Value = 0f;
            _inCooldown.Value = false;
        }

        private void OnEndAttack()
        {
            _inCooldown.Value = true;
            _currentCooldown.Value = _initialCooldown.Value;
        }
    }
}