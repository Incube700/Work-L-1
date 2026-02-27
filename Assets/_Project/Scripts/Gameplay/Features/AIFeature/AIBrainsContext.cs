using Assets._Project.Scripts.Gameplay.EntitiesCore;
using System;
using System.Collections.Generic;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature
{
    public sealed class AIBrainsContext : IDisposable
    {
        private readonly List<EntityToBrain> _items = new List<EntityToBrain>();

        public void SetFor(Entity entity, IBrain brain)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Entity == entity)
                {
                    _items[i].Brain.Disable();
                    _items[i].Brain.Dispose();

                    _items[i].Brain = brain;
                    _items[i].Brain.Enable();
                    return;
                }
            }

            _items.Add(new EntityToBrain(entity, brain));
            brain.Enable();
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Entity.IsInit == false)
                {
                    int last = _items.Count - 1;

                    _items[i].Brain.Disable();
                    _items[i].Brain.Dispose();
                    
                    _items[i] = _items[last];
                    _items.RemoveAt(last);
                    i--;
                    continue;
                }

                _items[i].Brain.Update(deltaTime);
            }
        }

        public void Dispose()
        {
            foreach (EntityToBrain item in _items)
            {
                item.Brain.Disable();
                item.Brain.Dispose();
                
            }

            _items.Clear();
        }

        private sealed class EntityToBrain
        {
            public Entity Entity;
            public IBrain Brain;

            public EntityToBrain(Entity entity, IBrain brain)
            {
                Entity = entity;
                Brain = brain;
            }
        }
    }
}