# Config Touch Points

This page lists where Graveyard Defense theme data should be assigned without rewriting gameplay code.

## Level Selection

| Asset/code | Current role | Theme touch point |
| --- | --- | --- |
| `Assets/_Project/Resources/Configs/DefendGame/DefendLevelsConfig.asset` | Runtime level list loaded by `ConfigService`. | Do not change for the local-only theme unless you are doing a temporary local test and will revert before commit. |
| `DefendLevelsConfig` | Holds `DefendLevelConfig[]`. | Can point to `GraveyardDefense_Level_01` only after a commit-safe asset policy exists. |
| `MainMenuPresenter` | Chooses a random level and opens gameplay. | No theme change needed. |
| `DefendGameplaySceneEntryPoint` | Direct scene fallback loads first configured level. | No theme change needed. |

## Level Config

| Config | Controls | Theme assignment |
| --- | --- | --- |
| `DefendLevelConfig` | Base/building config, player explosion config, mine/turret/puddle configs, waves, rewards, rest duration. | Local generated `GraveyardDefense_Level_01.asset` points to themed local parts and four hordes. |
| `WaveConfig` | Enemy config, enemy count, spawn interval, spawn radius. | Local generated horde waves point to zombie/ranged/heavy zombie configs. |

## Enemy Configs

| Config | Field | Theme use |
| --- | --- | --- |
| `EnemyConfigBase` | `_health`, `_moveSpeed`, `_prefabPath` | Point `_prefabPath` to local zombie wrappers such as `Entities/GraveyardDefense/ZombieEnemyView`. |
| `EnemyConfig` | `_explodeDistance`, `_explodeDamage` | Keep melee zombie behavior; tune only through config. |
| `ShooterEnemyConfig` | `_attackDistance`, `_attackInterval`, `_projectileConfig` | Keep shooter logic; use ranged undead visuals and the local purple projectile config. |

## Placeable And Building Configs

| Config | Field | Theme use |
| --- | --- | --- |
| `BuildingConfig` | `_health`, `_prefabPath` | Local ritual stone wrapper uses existing base/building behavior. |
| `MineConfig` | `_costGold`, radius/damage/timing fields, `_prefabPath` | Local rune wrapper keeps mine logic. |
| `TurretConfig` | `_costGold`, radius/attack interval/rotate speed/mask, `_prefabPath`, `_projectileConfig` | Local totem wrapper keeps turret logic and references local purple projectile config. |
| `PuddleConfig` | `_costGold`, radius/duration/tick/effect fields, `_prefabPath` | Local curse/swamp wrapper keeps puddle logic. |
| `ProjectileConfig` | `_prefabPath`, speed, lifetime, hit distance, collision radius, damage, explosion radius, mask | Local purple bolt config points to `Prefabs/GraveyardDefense/PurpleBoltProjectileView`. |

## Economy

| Config/code | Controls | Theme use |
| --- | --- | --- |
| `EconomyConfig` if present in active resources | Starting/current currency rules. | Keep values unless balancing the first playable. UI can display `Gold` as `Souls` without changing `CurrencyType.Gold`. |
| `CurrencyType.Gold` | Runtime gold identity. | Do not rename enum. Display as `Souls` through presentation config. |
| `CurrencyType.Diamond` | Runtime premium/reward identity. | Do not rename enum. Display as `Crystals` through presentation config. |

## UI Presentation

| Config/view | Controls | Theme use |
| --- | --- | --- |
| `DefendUiIconConfig` | Currency, placeable, phase, and enemy display names/icons. | Local generated config sets Souls, Crystals, Rune, Totem, Curse, Prepare, and Defend labels. |
| `CurrencyListView` / `CurrencyRowView` | Passive display of configured currency names/icons and amounts. | Keep view passive; assign presentation config through the existing scene path only if public clone policy allows references. |
| `PlacementPanelView` | Passive placement button/cost display and UI events. | Keep Mine/Turret/Puddle events; labels can read Rune/Totem/Curse from config. |
| `DefendHudView` | Passive phase/wave/base health display. | Keep HUD logic in presenter; display labels via presentation config where available. |

## Feedback Presentation

| Config/service | Controls | Theme use |
| --- | --- | --- |
| `DefendPresentationFeedbackConfig` | SFX/VFX cue lookups for presentation events. | Local generated config assigns MasterMagicFX VFX for preview, placement confirm, tower attack, and enemy death. |
| `DefendPresentationFeedbackConfig.DefendSfxCue` | Button, purchase, denied, wave, victory, defeat, placement, tower, base, enemy death SFX cues. | No local SFX assignment yet; add after license-safe SFX candidates are chosen. |
| `DefendPresentationFeedbackConfig.DefendVfxCue` | Placement preview, placeable confirm, wave start, tower attack, base hit, enemy death, player explosion VFX cues. | Use local MasterMagicFX assignments only in ignored configs. |

## Prefabs Currently Used

| Current prefab | Current usage | Themed local wrapper |
| --- | --- | --- |
| `Assets/_Project/Resources/Entities/DummyEnemy.prefab` | Melee enemy view/lifecycle/collider base. | `ZombieEnemyView`, `HeavyZombieEnemyView`. |
| `Assets/_Project/Resources/Entities/DummyEnemyShooter.prefab` | Ranged enemy view/lifecycle/collider base. | `RangedUndeadEnemyView`. |
| `Assets/_Project/Resources/Entities/DummyBuilding.prefab` | Base/building view/lifecycle/collider base. | `RitualStoneBaseView`. |
| `Assets/_Project/Resources/Entities/DummyMine.prefab` | Mine placeable view/lifecycle/collider base. | `MagicRuneMineView`. |
| `Assets/_Project/Resources/Entities/DummyTurret.prefab` | Turret placeable view/lifecycle/collider base. | `MagicTotemTurretView`. |
| `Assets/_Project/Resources/Entities/DummyPuddle.prefab` | Puddle placeable view/lifecycle/collider base. | `PoisonSwampPuddleView`. |
| `Assets/_Project/Resources/Prefabs/ProjectileFireball.prefab` | Projectile view/lifecycle/collider base. | `PurpleBoltProjectileView`. |

## Manual Unity Assignment

The local generated configs live under `_LocalGenerated`, which is ignored. Runtime still loads tracked configs from `Assets/_Project/Resources/Configs/DefendGame`.

For local testing only:

1. Run `Tools -> Defend -> Build Graveyard Defense Theme (Local)`.
2. Open the generated `Assets/_LocalGenerated/GraveyardDefense/Resources/Configs/DefendGame/GraveyardDefense/GraveyardDefense_Level_01.asset`.
3. Temporarily assign it through the gameplay entry path you are testing, or temporarily add it to tracked `DefendLevelsConfig.asset`.
4. Enter Play Mode and test Bootstrap -> MainMenu -> Gameplay.
5. Revert any tracked config/scene assignments before committing unless the referenced assets are allowed in the repo.
