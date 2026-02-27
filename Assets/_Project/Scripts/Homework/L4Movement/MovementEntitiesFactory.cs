using UnityEngine;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using Assets._Project.Scripts.Gameplay.Features.AttackFeature;

namespace Assets._Project.Scripts.Homework.L4Movement
{
    public sealed class MovementEntitiesFactory
    {
        private readonly EntitiesLifeContext _life;
        private readonly MonoEntitiesFactory _mono;
        private readonly IInputService _input;

        private readonly AttackFeatureInstaller _attackInstaller;

        public MovementEntitiesFactory(EntitiesLifeContext life, MonoEntitiesFactory mono, IInputService input)
        {
            _life = life;
            _mono = mono;
            _input = input;

            var attackSettings = new AttackSettings(0.35f, 0.10f, 0.25f);
            var attackInput = new AttackInputAdapter(_input);
            _attackInstaller = new AttackFeatureInstaller(attackSettings, attackInput);
        }

        public Entity CreateRigidbodyPlayer(Vector3 position)
        {
            Entity entity = new Entity();

            _mono.Create(entity, position, "Entities/PlayerRigidbody");

            entity.AddMoveSpeed(new ReactiveVariable<float>(10f));
            entity.AddMoveDirection(Vector3.zero);
            entity.AddRotationDirection(Vector3.forward);

            entity.AddSystem(new RigidbodyMovementSystem());
            entity.AddSystem(new RigidbodyRotationSystem());

            _attackInstaller.Install(entity);

            _life.Add(entity);
            return entity;
        }

        public Entity CreateCharacterControllerPlayer(Vector3 position)
        {
            Entity entity = new Entity();

            _mono.Create(entity, position, "Entities/PlayerCharacterController");

            entity.AddMoveSpeed(new ReactiveVariable<float>(10f));
            entity.AddMoveDirection(Vector3.zero);
            entity.AddRotationDirection(Vector3.forward);

            entity.AddSystem(new CharacterControllerMoveSystem());
            entity.AddSystem(new TransformRotationSystem());

            _attackInstaller.Install(entity);

            _life.Add(entity);
            return entity;
        }
    }
}