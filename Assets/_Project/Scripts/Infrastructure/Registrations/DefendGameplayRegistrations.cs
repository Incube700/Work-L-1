using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;
using Assets._Project.Scripts.Infrastructure.AssetsManagment;
using UnityEngine;

public static class DefendGameplayRegistrations
{
    public static void Register(IContainer container)
    {
        container.BindLazy<ResourcesAssetsLoader>(_ => new ResourcesAssetsLoader());

        container.BindLazy<EntitiesLifeContext>(_ => new EntitiesLifeContext());
        container.BindLazy<CollidersRegistryService>(_ => new CollidersRegistryService());

        container.BindLazy<MonoEntitiesFactory>(c =>
        {
            MonoEntitiesFactory monoFactory = new MonoEntitiesFactory(
                c.Resolve<ResourcesAssetsLoader>(),
                c.Resolve<EntitiesLifeContext>(),
                c.Resolve<CollidersRegistryService>());

            monoFactory.Initialize();
            return monoFactory;
        });

        container.BindLazy<DefendEntitiesFactory>(c => new DefendEntitiesFactory(
            c.Resolve<EntitiesLifeContext>(),
            c.Resolve<MonoEntitiesFactory>(),
            c.Resolve<ExplosionService>()));
            ;

        container.BindLazy<IInputService>(_ => new DesktopInputService());

        container.BindLazy<IPointerService>(c =>
        {
            DefendGameplaySceneData sceneData = c.Resolve<DefendGameplaySceneData>();

            return new DesktopPointerService(
                Camera.main,
                sceneData.GroundMask,
                sceneData.BuildingSpawnPoint.y);
        });

        container.BindLazy<ExplosionService>(c => new ExplosionService(
            c.Resolve<CollidersRegistryService>()));

        container.BindLazy<DefendPhaseService>(_ => new DefendPhaseService());

        container.BindLazy<WaveProgressService>(c => new WaveProgressService(
            c.Resolve<DefendLevelConfig>()));

        container.BindLazy<BuildingStateService>(_ => new BuildingStateService());
        container.BindLazy<EnemyService>(_ => new EnemyService());

        container.BindLazy<DefendResultService>(c => new DefendResultService(
            c.Resolve<DefendLevelConfig>(),
            c.Resolve<PlayerProgressService>(),
            c.Resolve<DefendPhaseService>()));

        container.BindLazy<PopupService>(c => new PopupService(
            c.Resolve<ViewsFactory>(),
            c.Resolve<ProjectPresentersFactory>(),
            c.Resolve<PopupLayer>().transform));

        container.BindTransient<DefendResultPresenter>(c => new DefendResultPresenter(
            c.Resolve<DefendResultService>(),
            c.Resolve<PopupService>(),
            c.Resolve<GameFlowService>(),
            c.Resolve<DefendLevelConfig>()));

        container.BindLazy<MineFactory>(c => new MineFactory(
            c.Resolve<DefendLevelConfig>(),
            c.Resolve<DefendEntitiesFactory>(),
            c.Resolve<EntitiesLifeContext>(),
            c.Resolve<ExplosionService>(),
            c.Resolve<CollidersRegistryService>()));

        container.BindLazy<MinePlacementService>(c => new MinePlacementService(
            c.Resolve<DefendLevelConfig>(),
            c.Resolve<WalletService>(),
            c.Resolve<MineFactory>()));
        
        container.BindLazy<ExplosionEffectService>(c => new ExplosionEffectService(
            c.Resolve<ExplosionService>(),
            c.Resolve<ResourcesAssetsLoader>().Load<GameObject>("VFX/PlayerClickEffect")));

        container.BindLazy<DefendInputHandler>(c => new DefendInputHandler(
            c.Resolve<IInputService>(),
            c.Resolve<IPointerService>(),
            c.Resolve<MinePlacementService>()));

        container.BindLazy<EnemySpawner>(c => new EnemySpawner(
            c.Resolve<DefendLevelConfig>(),
            c.Resolve<DefendEntitiesFactory>(),
            c.Resolve<BuildingStateService>(),
            c.Resolve<EnemyService>().Add));

        container.BindLazy<DefendStateMachineFactory>(c => new DefendStateMachineFactory(
            c.Resolve<DefendPhaseService>(),
            c.Resolve<WaveProgressService>(),
            c.Resolve<BuildingStateService>(),
            c.Resolve<DefendResultService>(),
            c.Resolve<EnemySpawner>(),
            c.Resolve<EnemyService>(),
            c.Resolve<DefendLevelConfig>().RestDurationSeconds));

        container.BindLazy<DefendStateMachine>(c => c.Resolve<DefendStateMachineFactory>().Create());

        container.BindTransient<DefendHudPresenter>(c => new DefendHudPresenter(
            c.Resolve<DefendHudView>(),
            c.Resolve<DefendPhaseService>(),
            c.Resolve<WaveProgressService>(),
            c.Resolve<BuildingStateService>()));

        container.BindTransient<CurrencyListPresenter>(c => new CurrencyListPresenter(
            c.Resolve<CurrencyListView>(),
            c.Resolve<ViewsFactory>(),
            c.Resolve<WalletService>()));
        
        container.BindLazy<DefendUiRuntime>(c => new DefendUiRuntime(
            c.Resolve<DefendHudPresenter>(),
            c.Resolve<CurrencyListPresenter>(),
            c.Resolve<DefendResultPresenter>(),
            c.Resolve<PopupService>()));

        container.BindLazy<DefendBuildingInitializer>(c => new DefendBuildingInitializer(
            c.Resolve<DefendLevelConfig>(),
            c.Resolve<DefendGameplaySceneData>(),
            c.Resolve<DefendEntitiesFactory>(),
            c.Resolve<BuildingStateService>(),
            c.Resolve<DefendInputHandler>(),
            c.Resolve<ExplosionService>()));

        container.BindLazy<DefendGameplayRuntime>(c => new DefendGameplayRuntime(
            c.Resolve<EntitiesLifeContext>(),
            c.Resolve<MonoEntitiesFactory>(),
            c.Resolve<IInputService>(),
            c.Resolve<BuildingStateService>(),
            c.Resolve<DefendResultService>(),
            c.Resolve<EnemyService>(),
            c.Resolve<DefendInputHandler>(),
            c.Resolve<DefendStateMachine>(),
            c.Resolve<WaveProgressService>(),
            c.Resolve<DefendPhaseService>(),
            c.Resolve<DefendUiRuntime>(),
            c.Resolve<DefendBuildingInitializer>(),
            c.Resolve<ExplosionEffectService>()));
    }
}
