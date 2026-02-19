using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.EntitiesCore
{
    public sealed class CollidersRegistryService
    {
        private readonly Dictionary<Collider, Entity> _colliderToEntity = new();

        public void Register(Collider collider, Entity entity)
        {
            if (collider == null)
                return;

            _colliderToEntity[collider] = entity;
        }

        public void Unregister(Collider collider)
        {
            if (collider == null)
                return;

            _colliderToEntity.Remove(collider);
        }

        public bool TryGetEntity(Collider collider, out Entity entity)
        {
            return _colliderToEntity.TryGetValue(collider, out entity);
        }
    }
}
