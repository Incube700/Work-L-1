using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Utilities.Conditions;
using System;

namespace Assets._Project.Scripts.Gameplay.Features.AttackFeature
{
    public sealed class StartAttackSystem : IInitializableSystem, IDisposableSystem
    {
        private SimpleEvent _startAttackRequest;
        private SimpleEvent _startAttackEvent;

        private ReactiveVariable<bool> _inAttackProcess;
        private ICompositeCondition _canStartAttack;

        private IDisposable _requestSubscription;

        public void OnInit(Entity entity)
        {
            _startAttackRequest = entity.StartAttackRequest;
            _startAttackEvent = entity.StartAttackEvent;

            _inAttackProcess = entity.InAttackProcess;
            _canStartAttack = entity.CanStartAttack;

            _requestSubscription = _startAttackRequest.Subscribe(OnStartAttackRequested);
        }

        public void OnDispose()
        {
            _requestSubscription?.Dispose();
            _requestSubscription = null;
        }

        private void OnStartAttackRequested()
        {
            if (_canStartAttack.Evaluate())
            {
                _inAttackProcess.Value = true;
                _startAttackEvent.Invoke();
            }
        }
    }
}