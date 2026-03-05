using System;
using System.Collections.Generic;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Targeting;
using Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Teleport;
using Assets._Project.Scripts.Gameplay.Features.TeleportFeature;
using Assets._Project.Scripts.Utilities.Conditions;
using Assets._Project.Scripts.Utilities.StateMachineCore;
using Assets._Project.Scripts.Utilities.Timers;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature
{
    public static class TeleportBrainFactory
    {
        public static IBrain CreateRandom(Entity entity, float interval)
        {
            CooldownTimer timer = new CooldownTimer(interval);
            ITeleportPositionCalculator calculator = new RandomTeleportPositionCalculator();

            EmptyState wait = new EmptyState(timer.Tick);
            TeleportState teleport = new TeleportState(entity, calculator);

            AIStateMachine stateMachine = new AIStateMachine(new List<IDisposable>
            {
                teleport.Entered.Subscribe(timer.Reset)
            });

            stateMachine.AddState(wait);
            stateMachine.AddState(teleport);

            stateMachine.AddTransition(wait, teleport, new CompositeCondition()
                .Add(new FuncCondition(() => timer.IsFinished))
                .Add(new FuncCondition(() => entity.CanTeleportCondition.IsSatisfied(entity)))
                .Add(new FuncCondition(() => calculator.CanCalculate(entity))));

            stateMachine.AddTransition(teleport, wait, new FuncCondition(() => true));
            return new StateMachineBrain(stateMachine);
        }

        public static IBrain CreateSmart(Entity entity, EntitiesLifeContext life, float interval, float minEnergyPercent)
        {
            CooldownTimer timer = new CooldownTimer(interval);
            ITeleportPositionCalculator calculator = new TargetTeleportPositionCalculator();

            FindTargetState findTarget = new FindTargetState(
                new LowestHealthTargetSelector(entity),
                life,
                entity,
                timer.Tick);

            TeleportState teleport = new TeleportState(entity, calculator);

            AIStateMachine stateMachine = new AIStateMachine(new List<IDisposable>
            {
                teleport.Entered.Subscribe(timer.Reset)
            });

            stateMachine.AddState(findTarget);
            stateMachine.AddState(teleport);

            stateMachine.AddTransition(findTarget, teleport, new CompositeCondition()
                .Add(new FuncCondition(() => timer.IsFinished))
                .Add(new FuncCondition(() => entity.CanTeleportCondition.IsSatisfied(entity)))
                .Add(new FuncCondition(() => HasEnoughEnergyPercent(entity, minEnergyPercent)))
                .Add(new FuncCondition(() => calculator.CanCalculate(entity))));

            stateMachine.AddTransition(teleport, findTarget, new FuncCondition(() => true));
            return new StateMachineBrain(stateMachine);
        }

        private static bool HasEnoughEnergyPercent(Entity entity, float minEnergyPercent)
        {
            float maxEnergy = entity.MaxEnergy.Value;

            if (maxEnergy <= 0f)
                return false;

            return entity.CurrentEnergy.Value / maxEnergy >= minEnergyPercent;
        }
    }
}
