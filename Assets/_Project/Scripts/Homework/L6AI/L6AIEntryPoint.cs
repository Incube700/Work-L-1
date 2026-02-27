using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.AIFeature;
using Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Targeting;
using Assets._Project.Scripts.Gameplay.Features.AIFeature.States.Teleport;
using Assets._Project.Scripts.Infrastructure.AssetsManagment;
using UnityEngine;

namespace Assets._Project.Scripts.Homework.L6AI
{
    public sealed class L6AIEntryPoint : MonoBehaviour
    {
        private const string TeleporterPath = "Entities/Teleporter";
        private const string DummyPath = "Entities/Dummy";

        [SerializeField] private Vector3 _spawnPosition;

        [Header("Teleporters")]
        [SerializeField] private float _randomInterval = 2f;
        [SerializeField] private float _smartInterval = 2f;
        [SerializeField, Range(0f, 1f)] private float _smartMinEnergyPercent = 0.4f;
        [SerializeField] private Vector3 _smartOffset = new Vector3(6f, 0f, 0f);

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

        private void Awake()
        {
            ResourcesAssetsLoader loader = new ResourcesAssetsLoader();

            _life = new EntitiesLifeContext();
            _collidersRegistry = new CollidersRegistryService();

            _monoFactory = new MonoEntitiesFactory(loader, _life, _collidersRegistry);
            _monoFactory.Initialize();

            _brains = new AIBrainsContext();

            L6AIEntitiesFactory factory = new L6AIEntitiesFactory(_life, _monoFactory, _collidersRegistry);

            // цели
            for (int i = 0; i < _targetsCount; i++)
            {
                Vector2 offset2 = UnityEngine.Random.insideUnitCircle * _targetsSpawnRadius;
                Vector3 pos = _spawnPosition + new Vector3(offset2.x, 0f, offset2.y);

                factory.CreateDummy(pos, DummyPath, _dummyHealth);
            }

            // Random
            Entity randomTeleporter = factory.CreateRandomTeleporter(_spawnPosition, TeleporterPath, _teleporterSettings);

            // Smart
            Entity smartTeleporter = factory.CreateSmartTeleporter(_spawnPosition + _smartOffset, TeleporterPath, _teleporterSettings);

            // brains
            {
                AIStateMachine sm = new AIStateMachine();
                sm.AddState(new RandomTeleportState(randomTeleporter, _randomInterval));

                _brains.SetFor(randomTeleporter, new StateMachineBrain(sm));
            }

            {
                LowestHealthTargetSelector selector = new LowestHealthTargetSelector(smartTeleporter);
                FindTargetState find = new FindTargetState(selector, _life, smartTeleporter);

                SmartTeleportState smart = new SmartTeleportState(smartTeleporter, _smartInterval, _smartMinEnergyPercent);

                AIParallelState rootParallel = new AIParallelState(find, smart);

                AIStateMachine root = new AIStateMachine();
                root.AddState(rootParallel);

                _brains.SetFor(smartTeleporter, new StateMachineBrain(root));
            }
        }

        private void Update()
        {
            float dt = Time.deltaTime;

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