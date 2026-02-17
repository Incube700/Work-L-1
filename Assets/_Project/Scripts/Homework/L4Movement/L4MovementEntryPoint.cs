using UnityEngine;
using Assets._Project.Scripts.Infrastructure.AssetsManagment;
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

        private EntitiesLifeContext _life;
        private Entity _player;
        private MonoEntitiesFactory _monoFactory;

        private void Awake()
        {
            ResourcesAssetsLoader loader = new ResourcesAssetsLoader();
            _life = new EntitiesLifeContext();

            _monoFactory = new MonoEntitiesFactory(loader, _life);
            _monoFactory.Initialize();

            MovementEntitiesFactory movementFactory = new MovementEntitiesFactory(_life, _monoFactory);

            _player = _mode == MovementMode.Rigidbody
                ? movementFactory.CreateRigidbodyPlayer(_spawnPosition)
                : movementFactory.CreateCharacterControllerPlayer(_spawnPosition);
        }

        private void Update()
        {
            // Инпут читаем в Update всегда (это важно)
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            _player.MoveDirection.Value = input;

            _life.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _life?.Dispose();
            _monoFactory?.Dispose();
        }
    }
}