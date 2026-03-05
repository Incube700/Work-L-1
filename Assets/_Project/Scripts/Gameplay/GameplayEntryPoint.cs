using System;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class GameplayEntryPoint : SceneEntryPointBase
{
    [Header("Defend mode scene refs")]
    [SerializeField] private Transform _defendBuildingSpawnPoint;
    [SerializeField] private LayerMask _defendGroundMask = ~0;

    private GameplayPresenter _presenter;
    private GameplayLoop _loop;
    private DefendGameplayRuntime _defendRuntime;
    private bool _isDefendMode;

    protected override void Register(IContainer container)
    {
        GameplayRegistrations.Register(container);
    }

    protected override void StartScene(IReadOnlyContainer container, SceneArgsService argsService)
    {
        if (argsService.TryGet(out DefendGameplayArgs defendArgs))
        {
            StartDefendGameplay(container, defendArgs);
            return;
        }

        if (argsService.TryGet(out GameplayArgs args) == false)
        {
            throw new InvalidOperationException("Gameplay args not found. Go to gameplay through menu.");
        }

        GameplayTargetView targetView = Object.FindObjectOfType<GameplayTargetView>(true);
        if (targetView == null)
            throw new InvalidOperationException("GameplayTargetView not found. Add it to TargetText in Gameplay scene.");

        GameplayTypedView typedView = Object.FindObjectOfType<GameplayTypedView>(true);
        if (typedView == null)
            throw new InvalidOperationException("GameplayTypedView not found. Add it to TypedText in Gameplay scene.");

        GameplayStatusView statusView = Object.FindObjectOfType<GameplayStatusView>(true);
        if (statusView == null)
            throw new InvalidOperationException("GameplayStatusView not found. Add it to StatusText in Gameplay scene.");

        CurrencyListView currencyListView = Object.FindObjectOfType<CurrencyListView>(true);
        if (currencyListView == null)
            throw new InvalidOperationException("CurrencyListView not found. Add CurrencyListView to Gameplay scene (CurrencyPanel).");

        ((IContainer)container).BindInstance(targetView);
        ((IContainer)container).BindInstance(typedView);
        ((IContainer)container).BindInstance(statusView);
        ((IContainer)container).BindInstance(currencyListView);

        // Сначала запускаем логику. UI должен быть просто подписчиком.
        _loop = container.Resolve<GameplayLoop>();
        _loop.Start(args.Mode);

        _presenter = container.Resolve<GameplayPresenter>();
        _presenter.Initialize();
    }

    private void Update()
    {
        if (_isDefendMode == false || _defendRuntime == null)
        {
            return;
        }

        _defendRuntime.Update(Time.deltaTime);
    }

    private void OnDestroy()
    {
        if (_defendRuntime != null)
        {
            _defendRuntime.Dispose();
            _defendRuntime = null;
            _isDefendMode = false;
        }

        if (_presenter != null)
        {
            // Сначала отписываем UI от событий, потом останавливаем луп.
            _presenter.Dispose();
            _loop.Stop();
            _presenter = null;
        }
    }

    private void StartDefendGameplay(IReadOnlyContainer container, DefendGameplayArgs args)
    {
        if (args.LevelConfig == null)
        {
            throw new InvalidOperationException("Defend level config is null.");
        }

        Debug.Log(
            $"[Defend] Start level: {args.LevelConfig.name}. BuildingHp={args.LevelConfig.BuildingHealth}, Waves={args.LevelConfig.Waves.Count}");

        WalletService wallet = container.Resolve<WalletService>();
        PlayerProgressService progress = container.Resolve<PlayerProgressService>();
        GameFlowService flow = container.Resolve<GameFlowService>();

        Vector3 spawnPoint = _defendBuildingSpawnPoint != null ? _defendBuildingSpawnPoint.position : Vector3.zero;

        _defendRuntime = new DefendGameplayRuntime(
            args.LevelConfig,
            spawnPoint,
            _defendGroundMask,
            wallet,
            progress,
            flow);

        _defendRuntime.Start();
        _isDefendMode = true;
    }
}
