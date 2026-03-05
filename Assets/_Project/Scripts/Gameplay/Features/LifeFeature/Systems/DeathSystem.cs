using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;

namespace Assets._Project.Scripts.Gameplay.Features.LifeFeature
{
    public sealed class DeathSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _currentHealth;
        private ReactiveVariable<bool> _isDead;

        public void OnInit(Entity entity)
        {
            _currentHealth = entity.CurrentHealth;
            _isDead = entity.IsDead;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isDead.Value)
                return;

            if (_currentHealth.Value > 0f)
                return;

            _currentHealth.Value = 0f;
            _isDead.Value = true;
        }
    }
}
