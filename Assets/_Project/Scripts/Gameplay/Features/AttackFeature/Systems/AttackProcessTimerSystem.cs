using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using System;

namespace Assets._Project.Scripts.Gameplay.Features.AttackFeature
{
    public sealed class AttackProcessTimerSystem : IInitializableSystem, IDisposableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<bool> _inAttackProcess;

        private SimpleEvent _startAttackEvent;
        private IDisposable _startSubscription;

        public void OnInit(Entity entity)
        {
            _currentTime = entity.AttackProcessCurrentTime;
            _inAttackProcess = entity.InAttackProcess;

            _startAttackEvent = entity.StartAttackEvent;
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

            _currentTime.Value += deltaTime;
        }

        private void OnStartAttack()
        {
            _currentTime.Value = 0f;
        }
    }
}