using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;

namespace Assets._Project.Scripts.Gameplay.Features.LifeFeature
{
    public sealed class SelfReleaseOnDeathSystem : IInitializableSystem, IUpdatableSystem
    {
        private readonly EntitiesLifeContext _life;

        private Entity _entity;
        private ReactiveVariable<bool> _isDead;

        private bool _released;

        public SelfReleaseOnDeathSystem(EntitiesLifeContext life)
        {
            _life = life;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _isDead = entity.IsDead;
            _released = false;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_released)
                return;

            if (_isDead.Value == false)
                return;

            _released = true;
            _life.Release(_entity);
        }
    }
}
