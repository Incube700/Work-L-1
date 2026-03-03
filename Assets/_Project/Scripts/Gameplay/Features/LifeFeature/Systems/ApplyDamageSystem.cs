using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.LifeFeature
{
    public sealed class ApplyDamageSystem : IInitializableSystem, IDisposableSystem
    {
        private Entity _entity;
        private ReactiveVariable<float> _currentHealth;
        private ReactiveVariable<bool> _isDead;
        private SimpleEvent<float> _takeDamageRequest;

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _currentHealth = entity.CurrentHealth;
            _isDead = entity.IsDead;
            _takeDamageRequest = entity.TakeDamageRequest;

            _takeDamageRequest.Invoked += OnTakeDamageRequested;
        }

        public void OnDispose()
        {
            if (_takeDamageRequest != null)
                _takeDamageRequest.Invoked -= OnTakeDamageRequested;

            _entity = null;
            _currentHealth = null;
            _isDead = null;
            _takeDamageRequest = null;
        }

        private void OnTakeDamageRequested(float damage)
        {
            if (_isDead.Value)
                return;

            if (damage <= 0f)
                return;

            float before = _currentHealth.Value;
            float next = before - damage;
            _currentHealth.Value = next;

            string name = _entity.Transform.name  ;
            Debug.Log($"[DMG] {name} -{damage} HP: {before} -> {next}");
        }
    }
}