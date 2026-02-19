using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.LifeFeature
{
    public sealed class DeathSystem : IInitializableSystem, IUpdatableSystem
    {
        private Entity _entity;
        private ReactiveVariable<float> _currentHealth;
        private ReactiveVariable<bool> _isDead;

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _currentHealth = entity.GetComponent<CurrentHealth>().Value;
            _isDead = entity.GetComponent<IsDead>().Value;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isDead.Value)
                return;

            if (_currentHealth.Value > 0f)
                return;

            _currentHealth.Value = 0f;
            _isDead.Value = true;

            string name = _entity.HasComponent<TransformComponent>() ? _entity.GetComponent<TransformComponent>().Value.name : "Unknown";
            Debug.Log($"[DEAD] {name}");
        }
    }
}