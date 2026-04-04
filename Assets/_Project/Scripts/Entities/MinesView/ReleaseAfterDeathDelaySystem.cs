using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;

public sealed class ReleaseAfterDeathDelaySystem : IInitializableSystem, IUpdatableSystem
{
    private readonly EntitiesLifeContext _life;
    private readonly float _delay;

    private Entity _entity;
    private ReactiveVariable<bool> _isDead;
    private float _timer;
    private bool _started;
    private bool _released;

    public ReleaseAfterDeathDelaySystem(EntitiesLifeContext life, float delay)
    {
        _life = life;
        _delay = delay;
    }

    public void OnInit(Entity entity)
    {
        _entity = entity;
        _isDead = entity.IsDead;
        _timer = 0f;
        _started = false;
        _released = false;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_released)
        {
            return;
        }

        if (_isDead.Value == false)
        {
            return;
        }

        if (_started == false)
        {
            _started = true;
            _timer = _delay;
        }

        _timer -= deltaTime;

        if (_timer > 0f)
        {
            return;
        }

        _released = true;
        _life.Release(_entity);
    }
}