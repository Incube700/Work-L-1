using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Infrastructure.AssetsManagment;
using UnityEngine;

namespace Assets._Project.Scripts.Homework.L5Teleport
{
    [Serializable]
    public class TeleportingCharacterSettings
    {
        [Min(1f)] public float MaxHealth = 100f;

        [Header("Energy")]
        [Min(1f)] public float MaxEnergy = 100f;
        [Min(0.1f)] public float EnergyRegenInterval = 2f;
        [Range(0f, 1f)] public float EnergyRegenPercent = 0.1f;

        [Header("Teleport")]
        [Min(0f)] public float TeleportRadius = 5f;
        [Min(0f)] public float TeleportEnergyCost = 20f;

        [Header("AoE")]
        [Min(0f)] public float AoEDamage = 25f;
        [Min(0f)] public float AoERadius = 3f;
    }

    public sealed class L5TeleportEntryPoint : MonoBehaviour
    {
        private const string TeleporterPath = "Entities/Teleporter";
        private const string DummyPath = "Entities/Dummy";

        [SerializeField] private Vector3 _spawnPosition;
        [SerializeField] private TeleportingCharacterSettings _teleporterSettings = new();

        [Header("Targets")]
        [SerializeField, Min(0)] private int _targetsCount = 6;
        [SerializeField, Min(0f)] private float _targetsSpawnRadius = 8f;
        [SerializeField, Min(1f)] private float _dummyHealth = 60f;

        private EntitiesLifeContext _life;
        private MonoEntitiesFactory _monoFactory;
        private CollidersRegistryService _collidersRegistry;

        private Entity _teleporter;

        private void Awake()
        {
            ResourcesAssetsLoader loader = new ResourcesAssetsLoader();

            // Контекст жизни сущностей (обновление + корректное уничтожение).
            _life = new EntitiesLifeContext();

            // Реестр Collider -> Entity нужен для AoE (из Physics мы получаем коллайдеры).
            _collidersRegistry = new CollidersRegistryService();

            // Фабрика MonoEntity: создаёт префабы и связывает их с Entity.
            _monoFactory = new MonoEntitiesFactory(loader, _life, _collidersRegistry);
            _monoFactory.Initialize();

            L5TeleportEntitiesFactory entitiesFactory = new L5TeleportEntitiesFactory(
                _life,
                _monoFactory,
                _collidersRegistry);

            _teleporter = entitiesFactory.CreateTeleportingCharacter(_spawnPosition, TeleporterPath, _teleporterSettings);

            for (int i = 0; i < _targetsCount; i++)
            {
                Vector2 offset2 = UnityEngine.Random.insideUnitCircle * _targetsSpawnRadius;
                Vector3 position = _spawnPosition + new Vector3(offset2.x, 0f, offset2.y);
                entitiesFactory.CreateDummy(position, DummyPath, _dummyHealth);
            }
        }

        private void Update()
        {
            // "Кнопка" (как в runtime TestGameplay): нажимаем T -> просим телепорт.
            if (Input.GetKeyDown(KeyCode.T))
                _teleporter.TeleportRequest.Invoke();

            // Для проверки смерти телепортера (не часть ТЗ, но удобно).
            if (Input.GetKeyDown(KeyCode.Space))
                _teleporter.TakeDamageRequest.Invoke(999f);

            _life.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _life?.Dispose();
            _monoFactory?.Dispose();
        }
    }
}
