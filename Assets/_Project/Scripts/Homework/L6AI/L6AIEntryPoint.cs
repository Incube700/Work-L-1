using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.AIFeature;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;
using Assets._Project.Scripts.Infrastructure.AssetsManagement;
using UnityEngine;

namespace Assets._Project.Scripts.Homework.L6AI
{
    public enum TeleportBrainMode
    {
        Random,
        Smart
    }

    public sealed class L6AIEntryPoint : MonoBehaviour
    {
        private const string TeleporterPath = "Entities/Teleporter";
        private const string DummyPath = "Entities/Dummy";

        [SerializeField] private Vector3 _spawnPosition;

        [Header("Teleporters")]
        [SerializeField] private float _randomInterval = 2f;
        [SerializeField] private float _smartInterval = 2f;
        [SerializeField, Range(0f, 1f)] private float _smartMinEnergyPercent = 0.4f;

        [Header("Brain Switching")]
        [SerializeField] private TeleportBrainMode _startBrainMode = TeleportBrainMode.Random;
        [SerializeField] private KeyCode _randomBrainKey = KeyCode.Alpha1;
        [SerializeField] private KeyCode _smartBrainKey = KeyCode.Alpha2;
        [SerializeField] private bool _enableBrainHotkeys = true;

        [Header("Targets")]
        [SerializeField, Min(0)] private int _targetsCount = 6;
        [SerializeField, Min(0f)] private float _targetsSpawnRadius = 8f;
        [SerializeField, Min(1f)] private float _dummyHealth = 60f;

        [Header("Settings")]
        [SerializeField] private Assets._Project.Scripts.Homework.L5Teleport.TeleportingCharacterSettings _teleporterSettings = new();

        private EntitiesLifeContext _life;
        private MonoEntitiesFactory _monoFactory;
        private CollidersRegistryService _collidersRegistry;

        private AIBrainsContext _brains;
        private IBrainSwitchInputService _brainInput;
        private Entity _teleporter;
        private bool _hasBrain;
        private TeleportBrainMode _currentBrainMode;

        private void Awake()
        {
            ResourcesAssetsLoader loader = new ResourcesAssetsLoader();

            _life = new EntitiesLifeContext();
            _collidersRegistry = new CollidersRegistryService();

            _monoFactory = new MonoEntitiesFactory(loader, _life, _collidersRegistry);
            _monoFactory.Initialize();

            _brains = new AIBrainsContext();
            _brainInput = new BrainSwitchInputService(_randomBrainKey, _smartBrainKey);

            L6AIEntitiesFactory factory = new L6AIEntitiesFactory(_life, _monoFactory, _collidersRegistry);

            // цели
            for (int i = 0; i < _targetsCount; i++)
            {
                Vector2 offset2 = UnityEngine.Random.insideUnitCircle * _targetsSpawnRadius;
                Vector3 pos = _spawnPosition + new Vector3(offset2.x, 0f, offset2.y);

                factory.CreateDummy(pos, DummyPath, _dummyHealth);
            }

            _teleporter = factory.CreateTeleporter(_spawnPosition, TeleporterPath, _teleporterSettings);
            SetBrain(_startBrainMode);
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            if (_enableBrainHotkeys)
            {
                _brainInput.Tick();

                if (_brainInput.RandomPressed)
                    SetBrain(TeleportBrainMode.Random);

                if (_brainInput.SmartPressed)
                    SetBrain(TeleportBrainMode.Smart);
            }

            _brains.Update(dt);
            _life.Update(dt);
        }

        private void OnDestroy()
        {
            _brains?.Dispose();
            _life?.Dispose();
            _monoFactory?.Dispose();
        }

        private void SetBrain(TeleportBrainMode mode)
        {
            if (_teleporter == null)
                return;

            if (_hasBrain && _currentBrainMode == mode)
                return;

            _currentBrainMode = mode;
            _hasBrain = true;

            IBrain brain = mode == TeleportBrainMode.Random
                ? TeleportBrainFactory.CreateRandom(_teleporter, _randomInterval)
                : TeleportBrainFactory.CreateSmart(_teleporter, _life, _smartInterval, _smartMinEnergyPercent);

            _brains.SetFor(_teleporter, brain);
        }
    }
}
