using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;
using UnityEngine;

public sealed class DefendGameplayRuntime : IDisposable
{
    private readonly EntitiesLifeContext _life;
    private readonly MonoEntitiesFactory _monoFactory;
    private readonly IInputService _input;
    private readonly BuildingStateService _buildingStateService;
    private readonly DefendResultService _resultService;
    private readonly EnemyService _enemyService;
    private readonly DefendInputHandler _inputHandler;
    private readonly DefendStateMachine _stateMachine;
    private readonly WaveProgressService _waveProgressService;
    private readonly DefendPhaseService _phaseService;
    private readonly DefendUiRuntime _uiRuntime;
    private readonly DefendBuildingInitializer _buildingInitializer;
    private readonly ExplosionEffectService _explosionEffectService;
    private readonly DefendPermanentUpgradesRuntime _permanentUpgradesRuntime;

    private bool _isStarted;

    public DefendGameplayRuntime(
        EntitiesLifeContext life,
        MonoEntitiesFactory monoFactory,
        IInputService input,
        BuildingStateService buildingStateService,
        DefendResultService resultService,
        EnemyService enemyService,
        DefendInputHandler inputHandler,
        DefendStateMachine stateMachine,
        WaveProgressService waveProgressService,
        DefendPhaseService phaseService,
        DefendUiRuntime uiRuntime,
        DefendBuildingInitializer buildingInitializer,
        ExplosionEffectService explosionEffectService,
        DefendPermanentUpgradesRuntime permanentUpgradesRuntime)
    {
        _life = life;
        _monoFactory = monoFactory;
        _input = input;
        _buildingStateService = buildingStateService;
        _resultService = resultService;
        _enemyService = enemyService;
        _inputHandler = inputHandler;
        _stateMachine = stateMachine;
        _waveProgressService = waveProgressService;
        _phaseService = phaseService;
        _uiRuntime = uiRuntime;
        _buildingInitializer = buildingInitializer;
        _explosionEffectService = explosionEffectService;
        _permanentUpgradesRuntime = permanentUpgradesRuntime;
    }

    public void Start()
    {
        if (_isStarted)
        {
            return;
        }

        _permanentUpgradesRuntime.Initialize();
        _buildingInitializer.Initialize();

        _life.Released += OnEntityReleased;

        _explosionEffectService.Initialize();
        _uiRuntime.Initialize();

        Log($"[Defend] Session started. Building HP: {_buildingStateService.MaxHealth}");

        if (_waveProgressService.HasAnyWaves == false)
        {
            _resultService.Win();
            _isStarted = true;
            return;
        }

        _stateMachine.Enter();

        _isStarted = true;
    }

    public void Update(float deltaTime)
    {
        if (_isStarted == false)
        {
            return;
        }

        _input.Tick();

        if (_phaseService.IsEnded == false)
        {
            _stateMachine.Update(deltaTime);

            float buildingY = _buildingStateService.HasBuilding
                ? _buildingStateService.Building.Transform.position.y
                : 0f;

            _inputHandler.Update(_phaseService.CurrentPhase, buildingY);
        }

        _life.Update(deltaTime);
    }

    public void Dispose()
    {
        if (_isStarted == false)
        {
            return;
        }

        _life.Released -= OnEntityReleased;

        _uiRuntime.Dispose();
        _buildingStateService.Dispose();
        _explosionEffectService.Dispose();
        _permanentUpgradesRuntime.Dispose();
        _stateMachine.Dispose();
        _enemyService.Dispose();
        _life.Dispose();
        _monoFactory.Dispose();

        _isStarted = false;
    }

    private void OnEntityReleased(Entity entity)
    {
        _enemyService.Remove(entity);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private static void Log(string message)
    {
        Debug.Log(message);
    }
}
