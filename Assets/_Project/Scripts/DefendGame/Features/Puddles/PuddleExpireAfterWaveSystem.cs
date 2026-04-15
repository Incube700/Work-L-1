using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;

public sealed class PuddleExpireAfterWaveSystem : IInitializableSystem, IUpdatableSystem
{
    private readonly EntitiesLifeContext _life;
    private readonly DefendPhaseService _phaseService;

    private Entity _entity;
    private bool _waveWasStarted;
    private bool _released;

    public PuddleExpireAfterWaveSystem(
        EntitiesLifeContext life,
        DefendPhaseService phaseService)
    {
        _life = life;
        _phaseService = phaseService;
    }

    public void OnInit(Entity entity)
    {
        _entity = entity;
        _waveWasStarted = false;
        _released = false;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_released)
        {
            return;
        }

        if (_phaseService.IsWave)
        {
            _waveWasStarted = true;
            return;
        }

        if (_waveWasStarted == false)
        {
            return;
        }

        if (_phaseService.IsRest == false)
        {
            return;
        }

        _released = true;
        _life.Release(_entity);
    }
}