using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.AttackFeature;
using Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Hero;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;
using Assets._Project.Scripts.Utilities.Conditions;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature
{
    public static class ManualHeroBrainFactory
    {
        private const float MoveThresholdSqr = 0.0001f;

        public static IBrain Create(Entity hero, IInputService input)
        {
            var idle = new ManualHeroIdleMouseRotateState(hero, input);
            var move = new ManualHeroMoveState(hero, input);
            var attack = new ManualHeroAttackState(hero);

            var sm = new AIStateMachine();
            sm.AddState(idle);
            sm.AddState(move);
            sm.AddState(attack);

            var canStartAttack = hero.CanStartAttack;

            sm.AddTransition(idle, move, new FuncCondition(() => input.Direction.sqrMagnitude > MoveThresholdSqr));
            sm.AddTransition(move, idle, new FuncCondition(() => input.Direction.sqrMagnitude <= MoveThresholdSqr));

            sm.AddTransition(idle, attack, new FuncCondition(() =>
                input.FireDown && canStartAttack.Evaluate()));

            // Бег всегда прерывает атаку
            sm.AddTransition(attack, move, new FuncCondition(() => input.Direction.sqrMagnitude > MoveThresholdSqr));

            // Атака закончилась/отменена → снова можно крутиться мышью
            sm.AddTransition(attack, idle, new FuncCondition(() =>
                hero.InAttackProcess.Value == false && input.Direction.sqrMagnitude <= MoveThresholdSqr));

            return new StateMachineBrain(sm);
        }
    }
}
