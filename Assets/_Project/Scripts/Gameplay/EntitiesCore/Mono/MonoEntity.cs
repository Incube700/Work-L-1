using Assets._Project.Scripts.Gameplay.EntitiesCore;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.EntitiesCore.Mono
{
    public class MonoEntity : MonoBehaviour
    {
        private CollidersRegistryService _collidersRegistryService;
        private Entity _linkedEntity;

        public Entity LinkedEntity => _linkedEntity;

        public void Initialize(CollidersRegistryService collidersRegistryService)
        {
            _collidersRegistryService = collidersRegistryService;
        }

        public void Setup(Entity entity)
        {
            _linkedEntity = entity;

            MonoEntityRegistrator[] registrators = GetComponentsInChildren<MonoEntityRegistrator>();

            if (registrators != null)
            {
                foreach (MonoEntityRegistrator registrator in registrators)
                {
                    registrator.Register(entity);
                }
            }

            if (_collidersRegistryService != null)
            {
                foreach (Collider collider in GetComponentsInChildren<Collider>())
                {
                    _collidersRegistryService.Register(collider, entity);
                }
            }
        }

        public void Cleanup(Entity entity)
        {
            if (_collidersRegistryService != null)
            {
                foreach (Collider collider in GetComponentsInChildren<Collider>())
                {
                    _collidersRegistryService.Unregister(collider);
                }
            }

            _linkedEntity = null;
        }
    }
}
