using UnityEngine;
using Assets._Project.Scripts.Gameplay.Infrastructure.DI;
using Assets._Project.Scripts.Gameplay.Infrastructure.AssetsManagment;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;

namespace Assets._Project.Scripts.Homework.L4Movement
{
    public enum MovementMode
    {
        Rigidbody,
        CharacterController
    }

    public sealed class L4MovementEntryPoint : MonoBehaviour
    {
        [SerializeField] private MovementMode _mode;
        [SerializeField] private Vector3 _spawnPosition;

        private DIContainer _container;
        private EntitiesLifeContext _life;
        private Entity _player;

        private void Awake()
        {
            _container = new DIContainer();

            _container.RegisterAsSingle(_ => new ResourcesAssetsLoader()).NonLazy();
            _container.RegisterAsSingle(_ => new EntitiesLifeContext()).NonLazy();

            _container.RegisterAsSingle(c => new MonoEntitiesFactory(
                c.Resolve<ResourcesAssetsLoader>(),
                c.Resolve<EntitiesLifeContext>())).NonLazy();

            _container.RegisterAsSingle(c => new MovementEntitiesFactory(
                c.Resolve<EntitiesLifeContext>(),
                c.Resolve<MonoEntitiesFactory>())).NonLazy();

            _container.Initialize();

            _life = _container.Resolve<EntitiesLifeContext>();
            MovementEntitiesFactory factory = _container.Resolve<MovementEntitiesFactory>();

            _player = _mode == MovementMode.Rigidbody
                ? factory.CreateRigidbodyPlayer(_spawnPosition)
                : factory.CreateCharacterControllerPlayer(_spawnPosition);
        }

        private void Update()
        {
            // Инпут читаем в Update всегда (это важно)
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            _player.MoveDirection.Value = input;

            if (_mode == MovementMode.CharacterController)
                _life.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (_mode == MovementMode.Rigidbody)
                _life.Update(Time.fixedDeltaTime);
        }

        private void OnDestroy()
        {
            _container?.Dispose();
        }
    }
}