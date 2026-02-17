using UnityEngine;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;

namespace Assets._Project.Scripts.Homework.L4Movement
{
    public sealed class MovementEntitiesFactory
    {
        private readonly EntitiesLifeContext _life;
        private readonly MonoEntitiesFactory _mono;

        public MovementEntitiesFactory(EntitiesLifeContext life, MonoEntitiesFactory mono)
        {
            _life = life;
            _mono = mono;
        }

        public Entity CreateRigidbodyPlayer(Vector3 position)
        {
            Entity entity = new Entity();

            _mono.Create(entity, position, "Entities/PlayerRigidbody");

            entity.AddMoveDirection();
            entity.AddMoveSpeed(new ReactiveVariable<float>(10f));

            entity.AddSystem(new RigidbodyMovementSystem());
            entity.AddSystem(new RigidbodyRotationSystem());

            _life.Add(entity);
            return entity;
        }

        public Entity CreateCharacterControllerPlayer(Vector3 position)
        {
            Entity entity = new Entity();

            _mono.Create(entity, position, "Entities/PlayerCharacterController");

            entity.AddMoveDirection();
            entity.AddMoveSpeed(new ReactiveVariable<float>(10f));

            entity.AddSystem(new CharacterControllerMoveSystem());
            entity.AddSystem(new TransformRotationSystem());

            _life.Add(entity);
            return entity;
        }
    }
}