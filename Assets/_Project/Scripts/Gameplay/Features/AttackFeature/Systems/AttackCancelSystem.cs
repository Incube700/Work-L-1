using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Utilities.Conditions;

namespace Assets._Project.Scripts.Gameplay.Features.AttackFeature
{
    public sealed class AttackCancelSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<bool> _inAttackProcess;
        private SimpleEvent _attackCanceledEvent;
        private ICompositeCondition _mustCancelAttack;

        public void OnInit(Entity entity)
        {
            _inAttackProcess = entity.GetComponent<InAttackProcess>().Value;
            _attackCanceledEvent = entity.GetComponent<AttackCanceledEvent>().Value;
            _mustCancelAttack = entity.GetComponent<MustCancelAttack>().Value;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inAttackProcess.Value == false)
                return;

            if (_mustCancelAttack.Evaluate())
            {
                _inAttackProcess.Value = false;
                _attackCanceledEvent.Invoke();
            }
        }
    }
}