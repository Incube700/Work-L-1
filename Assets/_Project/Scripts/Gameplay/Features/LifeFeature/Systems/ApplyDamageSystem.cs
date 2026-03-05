using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;

namespace Assets._Project.Scripts.Gameplay.Features.LifeFeature
{
    public sealed class ApplyDamageSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveVariable<float> _currentHealth;
        private ReactiveVariable<bool> _isDead;
        private SimpleEvent<float> _takeDamageRequest;

        public void OnInit(Entity entity)
        {
            _currentHealth = entity.CurrentHealth;
            _isDead = entity.IsDead;
            _takeDamageRequest = entity.TakeDamageRequest;

            _takeDamageRequest.Invoked += OnTakeDamageRequested;
        }

        public void OnDispose()
        {
            if (_takeDamageRequest != null)
                _takeDamageRequest.Invoked -= OnTakeDamageRequested;

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

            float next = _currentHealth.Value - damage;
            _currentHealth.Value = next;
        }
    }
}
