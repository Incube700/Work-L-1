using UnityEngine;
using Assets._Project.Scripts.Infrastructure.AssetsManagment;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.AIFeature;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;

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
        
        private IInputService _input;
        private AIBrainsContext _brains;
        
        private CollidersRegistryService _collidersRegistry;

        private void Awake()
        {
           ResourcesAssetsLoader loader = new ResourcesAssetsLoader();
           _life = new EntitiesLifeContext();
           
           _collidersRegistry = new CollidersRegistryService();
           
           _monoFactory = new MonoEntitiesFactory(loader, _life, _collidersRegistry);
           _monoFactory.Initialize();
           
            _input = new DesktopInputService();
            _brains = new AIBrainsContext();

            MovementEntitiesFactory movementFactory = new MovementEntitiesFactory(_life, _monoFactory, _input);

            _player = _mode == MovementMode.Rigidbody
                ? movementFactory.CreateRigidbodyPlayer(_spawnPosition)
                : movementFactory.CreateCharacterControllerPlayer(_spawnPosition);

            _brains.SetFor(_player, ManualHeroBrainFactory.Create(_player, _input));
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            
            _input.Tick();
            
            _brains.Update(dt);
            _life.Update(dt);
        }

        private void OnDestroy()
        {
            _brains?.Dispose();
            _life?.Dispose();
            _monoFactory?.Dispose();
        }
    }
}