# Theme Conversion Plan

The Graveyard Defense conversion should stay config/prefab-driven. The current Defend game loop, placement system, wave system, combat systems, presenters, and scene flow remain the authority for gameplay.

## Current First Playable Support

Run this Editor menu locally:

`Tools -> Defend -> Build Graveyard Defense Theme (Local)`

It creates ignored, local-only assets under:

`Assets/_LocalGenerated/GraveyardDefense/`

Generated local output includes:

| Output | Purpose |
| --- | --- |
| `Resources/Entities/GraveyardDefense/ZombieEnemyView.prefab` | Melee zombie wrapper based on the existing dummy melee enemy. |
| `Resources/Entities/GraveyardDefense/RangedUndeadEnemyView.prefab` | Ranged undead wrapper based on the existing shooter enemy. |
| `Resources/Entities/GraveyardDefense/HeavyZombieEnemyView.prefab` | Slow, higher-health zombie wrapper based on the existing melee enemy. |
| `Resources/Entities/GraveyardDefense/MagicRuneMineView.prefab` | Visual wrapper for `MineConfig` behavior. |
| `Resources/Entities/GraveyardDefense/MagicTotemTurretView.prefab` | Visual wrapper for `TurretConfig` behavior. |
| `Resources/Entities/GraveyardDefense/PoisonSwampPuddleView.prefab` | Visual wrapper for `PuddleConfig` behavior. |
| `Resources/Entities/GraveyardDefense/RitualStoneBaseView.prefab` | Visual wrapper for `BuildingConfig` behavior. |
| `Resources/Prefabs/GraveyardDefense/PurpleBoltProjectileView.prefab` | Projectile visual wrapper based on the existing projectile prefab. |
| `Resources/Configs/DefendGame/GraveyardDefense/GraveyardDefense_Level_01.asset` | Local 4-horde test level. |
| `Resources/Configs/DefendGame/GraveyardDefense/GraveyardDefenseLevelsConfig.asset` | Local level list for manual test injection. |
| `Resources/Configs/DefendGame/GraveyardDefense/GraveyardDefenseUiIconConfig.asset` | Local labels for Souls, Crystals, Rune, Totem, Curse, Prepare, and Defend. |
| `Resources/Configs/DefendGame/GraveyardDefense/GraveyardDefenseFeedbackConfig.asset` | Local MasterMagicFX VFX cue assignments. |

These generated assets may reference `Toon_Zombies_extended`, `MasterMagicFX`, and other local packs. Keep them ignored and out of commits.

## Safe Conversion Path

1. Use the local builder to generate the ignored wrapper prefabs and configs.
2. Inspect generated wrappers in Unity. Confirm each wrapper still has the original required `MonoEntity`, colliders, registrators, animator hooks, and entity lifecycle components from the base prefab.
3. Test `GraveyardDefense_Level_01.asset` manually by assigning it to the gameplay entry path for local testing only.
4. Keep the tracked `DefendLevelsConfig` unchanged until the asset license policy is clear.
5. If a public-safe art set is chosen later, create tracked project-owned wrappers under `Assets/_Project/Resources/...` and update tracked configs in a separate commit.

## What Stays In Code

Gameplay rules stay in existing systems:

| System area | Keep current owner |
| --- | --- |
| Enemy spawning and wave timing | Wave configs and runtime wave services. |
| Enemy movement, attack, death | Existing Defend enemy factories/systems. |
| Placement decisions and affordability | Placement presenters/services/runtime systems. |
| Tower targeting and projectile creation | Turret/projectile factories and systems. |
| Mine and puddle effects | Existing placeable configs and runtime systems. |
| Currency and rewards | Existing economy configs/services. |
| Result flow | Existing gameplay state/presenter flow. |

Do not move any of that logic into wrapper prefabs, UI views, VFX prefabs, or art scripts.

## Visual Replacement Rules

- Enemy visuals are changed by `EnemyConfigBase.PrefabPath`.
- Building visuals are changed by `BuildingConfig.PrefabPath`.
- Mine visuals are changed by `MineConfig.PrefabPath`.
- Turret visuals are changed by `TurretConfig.PrefabPath`.
- Puddle visuals are changed by `PuddleConfig.PrefabPath`.
- Projectile visuals are changed by `ProjectileConfig.PrefabPath`.
- UI labels/icons are changed by `DefendUiIconConfig` when the active scene/view has that config assigned.
- VFX/SFX are changed by `DefendPresentationFeedbackConfig` where current presenters/services already read feedback cues.

## Not In This Pass

- No state machine rewrite.
- No placement system rewrite.
- No wave system rewrite.
- No combat rewrite.
- No permanent rename of enums/classes/serialized fields.
- No tracked scene or prefab references to local-only paid packs.
- No vendor asset modification.

## Later Commit-Safe Path

After asset licenses are confirmed, choose one:

| Option | Commit safety |
| --- | --- |
| Keep local-only generated assets | Good for private machine demos only; do not commit generated asset references. |
| Replace local packs with redistributable art | Safe for public GitHub when license permits redistribution. |
| Track wrapper prefabs but leave vendor references missing | Usually bad for public clones; only use if the team accepts missing references. |
| Create project-owned placeholder wrappers | Safe fallback for public clones, but lower visual quality until licensed art is integrated privately. |
