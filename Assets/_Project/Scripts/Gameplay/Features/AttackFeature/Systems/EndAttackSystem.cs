using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;

namespace Assets._Project.Scripts.Gameplay.Features.AttackFeature
{
    public sealed class EndAttackSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<bool> _inAttackProcess;
        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<float> _initialTime;

        private SimpleEvent _endAttackEvent;

        public void OnInit(Entity entity)
        {
            _inAttackProcess = entity.InAttackProcess;

            _currentTime = entity.AttackProcessCurrentTime;
            _initialTime = entity.AttackProcessInitialTime;

            _endAttackEvent = entity.EndAttackEvent;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inAttackProcess.Value == false)
                return;

            if (_currentTime.Value < _initialTime.Value)
                return;

            _inAttackProcess.Value = false;
            _endAttackEvent.Invoke();
        }
    }
}