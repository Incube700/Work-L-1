using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Utilities.Conditions;

namespace Assets._Project.Scripts.Gameplay.Features.AttackFeature
{
    public sealed class AttackFeatureInstaller
    {
        private const float MoveThresholdSqr = 0.0001f;

        private readonly AttackSettings _settings;
        private readonly IAttackInput _input;

        public AttackFeatureInstaller(AttackSettings settings, IAttackInput input)
        {
            _settings = settings;
            _input = input;
        }

        public void Install(Entity entity)
        {
            AddComponents(entity);
            AddConditions(entity);
            AddSystems(entity);
        }

        private void AddComponents(Entity entity)
        {
            entity.AddStartAttackRequest(new SimpleEvent());
            entity.AddStartAttackEvent(new SimpleEvent());
            entity.AddEndAttackEvent(new SimpleEvent());
            entity.AddAttackDelayEndEvent(new SimpleEvent());
            entity.AddAttackCanceledEvent(new SimpleEvent());

            entity.AddAttackProcessInitialTime(_settings.AttackDuration);
            entity.AddAttackProcessCurrentTime(0f);
            entity.AddInAttackProcess(false);

            entity.AddAttackDelayTime(_settings.AttackDelay);

            entity.AddAttackCooldownInitialTime(_settings.AttackCooldown);
            entity.AddAttackCooldownCurrentTime(0f);
            entity.AddInAttackCooldown(false);
        }

        private void AddConditions(Entity entity)
        {
            ICompositeCondition canStartAttack = new CompositeCondition()
                .Add(new FuncCondition(() => _input.MoveSqrMagnitude <= MoveThresholdSqr))
                .Add(new FuncCondition(() => entity.InAttackProcess.Value == false))
                .Add(new FuncCondition(() => entity.InAttackCooldown.Value == false));

            entity.AddCanStartAttack(canStartAttack);

            ICompositeCondition mustCancelAttack = new CompositeCondition()
                .Add(new FuncCondition(() => _input.MoveSqrMagnitude > MoveThresholdSqr));

            entity.AddMustCancelAttack(mustCancelAttack);
        }

        private void AddSystems(Entity entity)
        {
            entity.AddSystem(new StartAttackSystem());
            entity.AddSystem(new AttackProcessTimerSystem());
            entity.AddSystem(new AttackDelayEndTriggerSystem());
            entity.AddSystem(new EndAttackSystem());
            entity.AddSystem(new AttackCooldownTimerSystem());
            entity.AddSystem(new AttackCancelSystem());
            entity.AddSystem(new AttackDebugFeedbackSystem());
        }
    }
}
