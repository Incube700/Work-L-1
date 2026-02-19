using Assets._Project.Scripts.Infrastructure.AssetsManagment;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets._Project.Scripts.Gameplay.EntitiesCore.Mono
{
    public class MonoEntitiesFactory : IDisposable
    {
        private readonly ResourcesAssetsLoader _resources;
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly CollidersRegistryService _collidersRegistryService;

        private readonly Dictionary<Entity, MonoEntity> _entityToMono = new();
        private readonly List<Entity> _tempEntities = new();

        public MonoEntitiesFactory(
            ResourcesAssetsLoader resources,
            EntitiesLifeContext entitiesLifeContext,
            CollidersRegistryService collidersRegistryService = null)
        {
            _resources = resources;
            _entitiesLifeContext = entitiesLifeContext;
            _collidersRegistryService = collidersRegistryService;
        }

        public MonoEntity Create(Entity entity, Vector3 position, string path)
        {
            MonoEntity prefab = _resources.Load<MonoEntity>(path);

            MonoEntity viewInstance = Object.Instantiate(prefab, position, Quaternion.identity, null);
            viewInstance.Initialize(_collidersRegistryService);
            viewInstance.Setup(entity);

            _entityToMono.Add(entity, viewInstance);

            return viewInstance;
        }

        public void Initialize()
        {
            _entitiesLifeContext.Released += OnEntityReleased;
        }

        public void Dispose()
        {
            _entitiesLifeContext.Released -= OnEntityReleased;

            _tempEntities.Clear();

            foreach (Entity entity in _entityToMono.Keys)
                _tempEntities.Add(entity);

            for (int i = 0; i < _tempEntities.Count; i++)
                CleanupFor(_tempEntities[i]);

            _entityToMono.Clear();
        }

        private void OnEntityReleased(Entity entity)
        {
            CleanupFor(entity);
        }

        private void CleanupFor(Entity entity)
        {
            if (_entityToMono.TryGetValue(entity, out MonoEntity monoEntity) == false)
                return;

            _entityToMono.Remove(entity);

            // Unity-объект мог быть уже уничтожен 
            if (monoEntity == null)
                return;

            monoEntity.Cleanup(entity);

            if (monoEntity.gameObject != null)
                Object.Destroy(monoEntity.gameObject);
        }
    }
}
