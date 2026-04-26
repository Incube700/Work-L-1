using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.InputFeature;
using Assets._Project.Scripts.Infrastructure.AssetsManagement;
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
            c.Resolve<ExplosionService>(),
            c.Resolve<ProjectileFactory>()));

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
        
        container.BindLazy<RestTimerService>(_ => new RestTimerService());

        container.BindLazy<WaveProgressService>(c => new WaveProgressService(
            c.Resolve<DefendLevelConfig>()));

        container.BindLazy<BuildingStateService>(_ => new BuildingStateService());
        container.BindLazy<EnemyService>(_ => new EnemyService());
        container.BindLazy<BuildingCombatService>(c => new BuildingCombatService(
            c.Resolve<BuildingStateService>()));

        container.BindLazy<DefendPermanentUpgradesRuntime>(c => new DefendPermanentUpgradesRuntime(
            c.Resolve<PermanentUpgradesService>(),
            c.Resolve<ConfigService>(),
            c.Resolve<BuildingStateService>(),
            c.Resolve<WaveProgressService>(),
            c.Resolve<EnemyService>()));

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
        
        container.BindLazy<ProjectileFactory>(c => new ProjectileFactory(
            c.Resolve<MonoEntitiesFactory>(),
            c.Resolve<EntitiesLifeContext>(),
            c.Resolve<CollidersRegistryService>(),
            c.Resolve<ExplosionService>()));
        
        container.BindLazy<TurretFactory>(c => new TurretFactory(
            c.Resolve<DefendLevelConfig>(),
            c.Resolve<DefendEntitiesFactory>(),
            c.Resolve<EntitiesLifeContext>(),
            c.Resolve<CollidersRegistryService>(),
            c.Resolve<ProjectileFactory>()));

        container.BindLazy<PuddleFactory>(c => new PuddleFactory(
            c.Resolve<DefendLevelConfig>(),
            c.Resolve<DefendEntitiesFactory>(),
            c.Resolve<EntitiesLifeContext>(),
            c.Resolve<CollidersRegistryService>(),
            c.Resolve<DefendPhaseService>()));

        container.BindLazy<TurretPlacementService>(c => new TurretPlacementService(
            c.Resolve<DefendLevelConfig>(),
            c.Resolve<WalletService>(),
            c.Resolve<TurretFactory>()));

        container.BindLazy<PuddlePlacementService>(c => new PuddlePlacementService(
            c.Resolve<DefendLevelConfig>(),
            c.Resolve<WalletService>(),
            c.Resolve<PuddleFactory>()));
        
        container.BindLazy<PlacementSelectionService>(_ => new PlacementSelectionService());
        
        container.BindLazy<PlacementService>(c => new PlacementService(
            c.Resolve<PlacementSelectionService>(),
            c.Resolve<MinePlacementService>(),
            c.Resolve<TurretPlacementService>(),
            c.Resolve<PuddlePlacementService>()));
        
        container.BindLazy<ExplosionEffectService>(c => new ExplosionEffectService(
            c.Resolve<ExplosionService>(),
            c.Resolve<ResourcesAssetsLoader>().Load<GameObject>("VFX/PlayerClickEffect")));

        container.BindLazy<DefendInputHandler>(c => new DefendInputHandler(
            c.Resolve<IInputService>(),
            c.Resolve<IPointerService>(),
            c.Resolve<IUiPointerBlockService>(),
            c.Resolve<PlacementService>(),
            c.Resolve<BuildingCombatService>()));

        container.BindLazy<EnemySpawner>(c => new EnemySpawner(
            c.Resolve<DefendLevelConfig>(),
            c.Resolve<DefendEntitiesFactory>(),
            c.Resolve<BuildingStateService>(),
            c.Resolve<EnemyService>().Add));

        container.BindLazy<DefendStateMachineFactory>(c => new DefendStateMachineFactory(
            c.Resolve<DefendPhaseService>(),
            c.Resolve<RestTimerService>(),
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
            c.Resolve<RestTimerService>(),
            c.Resolve<WaveProgressService>(),
            c.Resolve<BuildingStateService>()));
        
        container.BindTransient<PlacementPanelPresenter>(c => new PlacementPanelPresenter(
            c.Resolve<PlacementPanelView>(),
            c.Resolve<PlacementSelectionService>(),
            c.Resolve<DefendPhaseService>(),
            c.Resolve<DefendLevelConfig>()));
        
        container.BindTransient<DefendGameplayScreenPresenter>(c => new DefendGameplayScreenPresenter(
            c.Resolve<DefendGameplayScreenView>(),
            c.Resolve<DefendHudPresenter>(),
            c.Resolve<CurrencyListPresenter>(),
            c.Resolve<PlacementPanelPresenter>(),
            c.Resolve<DefendResultPresenter>()));

        container.BindTransient<CurrencyListPresenter>(c => new CurrencyListPresenter(
            c.Resolve<CurrencyListView>(),
            c.Resolve<ViewsFactory>(),
            c.Resolve<WalletService>()));
        
        container.BindLazy<DefendUiRuntime>(c => new DefendUiRuntime(
            c.Resolve<DefendGameplayScreenPresenter>(),
            c.Resolve<PopupService>()));
        
        container.BindLazy<IUiPointerBlockService>(_ => new UnityUiPointerBlockService());

        container.BindLazy<DefendBuildingInitializer>(c => new DefendBuildingInitializer(
            c.Resolve<DefendLevelConfig>(),
            c.Resolve<DefendGameplaySceneData>(),
            c.Resolve<DefendEntitiesFactory>(),
            c.Resolve<BuildingStateService>(),
            c.Resolve<BuildingCombatService>(),
            c.Resolve<ExplosionService>(),
            c.Resolve<DefendPermanentUpgradesRuntime>()));

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
            c.Resolve<ExplosionEffectService>(),
            c.Resolve<DefendPermanentUpgradesRuntime>()));
    }
}
