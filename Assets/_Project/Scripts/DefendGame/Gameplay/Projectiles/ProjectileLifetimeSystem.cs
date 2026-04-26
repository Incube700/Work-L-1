using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;

public sealed class ProjectileLifetimeSystem : IInitializableSystem, IUpdatableSystem
{
    private ReactiveVariable<float> _lifeTime;
    private ReactiveVariable<bool> _isDead;

    public void OnInit(Entity entity)
    {
        _lifeTime = entity.GetComponent<ProjectileLifetime>().Value;
        _isDead = entity.IsDead;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_isDead.Value)
        {
            return;
        }

        _lifeTime.Value -= deltaTime;

        if (_lifeTime.Value > 0f)
        {
            return;
        }

        _isDead.Value = true;
    }
}