using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.EnergyFeature;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using Assets._Project.Scripts.Gameplay.Features.TeleportFeature;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Conditions;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature.Conditions;
using Assets._Project.Scripts.Gameplay.Features.TeleportFeature.Conditions;
using UnityEngine;

namespace Assets._Project.Scripts.Homework.L5Teleport
{
    public sealed class L5TeleportEntitiesFactory
    {
        private readonly EntitiesLifeContext _life;
        private readonly MonoEntitiesFactory _mono;
        private readonly CollidersRegistryService _collidersRegistry;

        public L5TeleportEntitiesFactory(
            EntitiesLifeContext life,
            MonoEntitiesFactory mono,
            CollidersRegistryService collidersRegistry)
        {
            _life = life;
            _mono = mono;
            _collidersRegistry = collidersRegistry;
        }

        public Entity CreateTeleportingCharacter(Vector3 position, string prefabPath, TeleportingCharacterSettings settings)
        {
            Entity entity = new Entity();
            MonoEntity view = _mono.Create(entity, position, prefabPath);
            
            entity.AddTransform(view.transform);

            entity.AddMaxHealth(settings.MaxHealth);
            entity.AddCurrentHealth(settings.MaxHealth);
            entity.AddIsDead(false);
            entity.AddTakeDamageRequest(new SimpleEvent<float>());

            entity.AddMaxEnergy(settings.MaxEnergy);
            entity.AddCurrentEnergy(settings.MaxEnergy);
            entity.AddEnergyRegenInterval(settings.EnergyRegenInterval);
            entity.AddEnergyRegenPercent(settings.EnergyRegenPercent);
            entity.AddEnergyRegenTimer(0f);

            entity.AddTeleportRadius(settings.TeleportRadius);
            entity.AddTeleportEnergyCost(settings.TeleportEnergyCost);
            entity.AddTeleportRequest(new SimpleEvent<Vector3>());
            entity.AddTeleportedEvent(new SimpleEvent());

            entity.AddTeleportAoEDamage(settings.AoEDamage);
            entity.AddTeleportAoERadius(settings.AoERadius);
            entity.AddTeleportAoEMask(~0);

            ICondition canTeleport = new AllConditions(
                new IsAliveCondition(),
                new HasEnoughEnergyForTeleportCondition());

            entity.AddCanTeleportCondition(canTeleport);

            ICondition canRegenEnergy = new IsAliveCondition();
            
            entity.AddSystem(new ApplyDamageSystem());
            entity.AddSystem(new DeathSystem());
            entity.AddSystem(new EnergyRegenerationSystem(canRegenEnergy));
            entity.AddSystem(new TeleportSystem(canTeleport));
            entity.AddSystem(new SpendEnergyOnTeleportedSystem());
            entity.AddSystem(new TeleportAoEDamageSystem(_collidersRegistry));
            entity.AddSystem(new SelfReleaseOnDeathSystem(_life));

            _life.Add(entity);
            return entity;
        }

        public Entity CreateDummy(Vector3 position, string prefabPath, float health)
        {
            Entity entity = new Entity();
            MonoEntity view = _mono.Create(entity, position, prefabPath);
            entity.AddTransform(view.transform);

            entity.AddMaxHealth(health);
            entity.AddCurrentHealth(health);
            entity.AddIsDead(false);
            entity.AddTakeDamageRequest(new SimpleEvent<float>());

            entity.AddSystem(new ApplyDamageSystem());
            entity.AddSystem(new DeathSystem());
            entity.AddSystem(new SelfReleaseOnDeathSystem(_life));

            _life.Add(entity);
            return entity;
        }
    }
}
