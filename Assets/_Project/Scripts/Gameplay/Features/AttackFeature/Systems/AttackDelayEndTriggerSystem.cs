using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using System;

namespace Assets._Project.Scripts.Gameplay.Features.AttackFeature
{
    public sealed class AttackDelayEndTriggerSystem : IInitializableSystem, IDisposableSystem, IUpdatableSystem
    {
        private SimpleEvent _delayEndEvent;
        private SimpleEvent _startAttackEvent;

        private ReactiveVariable<float> _delay;
        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<bool> _inAttackProcess;

        private bool _alreadyTriggered;

        private IDisposable _startSubscription;

        public void OnInit(Entity entity)
        {
            _delayEndEvent = entity.GetComponent<AttackDelayEndEvent>().Value;
            _startAttackEvent = entity.GetComponent<StartAttackEvent>().Value;

            _delay = entity.GetComponent<AttackDelayTime>().Value;
            _currentTime = entity.GetComponent<AttackProcessCurrentTime>().Value;
            _inAttackProcess = entity.GetComponent<InAttackProcess>().Value;

            _startSubscription = _startAttackEvent.Subscribe(OnStartAttack);
        }

        public void OnDispose()
        {
            _startSubscription?.Dispose();
            _startSubscription = null;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inAttackProcess.Value == false)
                return;

            if (_alreadyTriggered)
                return;

            if (_currentTime.Value >= _delay.Value)
            {
                _alreadyTriggered = true;
                _delayEndEvent.Invoke();
            }
        }

        private void OnStartAttack()
        {
            _alreadyTriggered = false;
        }
    }
}