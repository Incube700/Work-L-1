# Architecture

## Scene Flow

The active tower-defence flow is:

1. `Assets/_Project/Scenes/BootstrapScene.unity`
2. `Assets/_Project/Scenes/MainMenuScene.unity`
3. `Assets/_Project/Scenes/GameplayScene.unity`

`BootstrapScene` owns `ProjectContext`, registers project-level services, loads saves, and opens the main menu. `MainMenuScene` uses `MainMenuEntryPoint`. `GameplayScene` uses `DefendGameplayEntryPoint`.

## Project Infrastructure

- `ProjectContext` initializes shared project services.
- `SceneEntryPointBase` provides scene registration/start structure.
- `SceneLoader`, `SceneArgsService`, and `GameFlowService` own scene transitions and scene arguments.
- `ConfigService` loads project-level configs from `Resources`.
- `SaveService` coordinates save providers for stats, wallet, and permanent upgrades.
- `Container`/`IContainer` provide lightweight dependency registration and resolution.

## Defend Gameplay Runtime

Active code is mainly under `Assets/_Project/Scripts/DefendGame`.

- `DefendGameplayEntryPoint` validates scene references, binds scene objects, resolves the runtime, and starts it.
- `DefendGameplayRuntime` initializes permanent upgrades, building state, VFX service, UI runtime, and the state machine.
- `DefendStateMachine` coordinates rest, wave, win, and lose states.
- `EnemySpawner`, factories, and entity systems own object creation and gameplay updates.
- `DefendResultService` owns win/lose state changes and rewards.
- `PlacementService` routes placement requests to mine, turret, and puddle placement services.

Entry points remain thin enough for the current prototype: they validate serialized scene references, bind them into DI, and start the runtime.

## Active UI

Main menu:

- `MainMenuScreenView`
- `MainMenuView`
- `MainMenuPresenter`
- `PermanentUpgradesMenuView`
- `PermanentUpgradesMenuPresenter`
- `StatsView`
- `StatsPresenter`
- `CurrencyListView`
- `CurrencyListPresenter`

Gameplay:

- `DefendGameplayScreenView`
- `DefendGameplayScreenPresenter`
- `DefendHudView`
- `DefendHudPresenter`
- `PlacementPanelView`
- `PlacementPanelPresenter`
- `CurrencyListView`
- `CurrencyListPresenter`
- `MessagePopupView`
- `MessagePopupPresenter`

UI views remain passive. They display data, expose serialized Unity UI references, and raise events. Presenters own subscriptions and translate services/config data into display state.

## UI Prefabs And Scene UI

Resource-loaded UI prefabs:

- `Assets/_Project/Resources/UI/Currency/CurrencyRowView.prefab`
- `Assets/_Project/Resources/UI/Popups/MessagePopupView.prefab`

Scene-owned UI:

- Gameplay HUD, placement panel, currency panel, and popup layer are in `GameplayScene`.
- Main menu, stats, permanent upgrade menu, currency panel, and popup layer are in `MainMenuScene`.

Manual scene/prefab edits are risky because these references are serialized in Unity YAML. Prefer code changes that use existing serialized references, or document Unity Editor steps when new references are required.

## Configs

Project-level configs:

- `EconomyConfig`
- `GameModesConfig`
- `PermanentUpgradesConfig`
- `DefendLevelsConfig`

Defend gameplay configs:

- `DefendLevelConfig`
- `WaveConfig`
- `BuildingConfig`
- `EnemyConfig`
- `ShooterEnemyConfig`
- `MineConfig`
- `TurretConfig`
- `PuddleConfig`
- `ProjectileConfig`
- `PlayerExplosionConfig`

Presentation configs prepared in this pass:

- `DefendUiIconConfig`
- `DefendPresentationFeedbackConfig`

The new presentation config classes are intentionally not wired into the runtime yet. They provide small, safe config types for future UI icon, label, VFX, and SFX mapping after Unity Editor asset assignment.
