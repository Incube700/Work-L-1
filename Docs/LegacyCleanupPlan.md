# Legacy Cleanup Plan

No legacy files are deleted in this pass.

## Safe Candidates

- Old typing-game UI and gameplay under `Assets/_Project/Scripts/UI/HUD`, `Assets/_Project/Scripts/UI/GameplayPresenter.cs`, and `Assets/_Project/Scripts/Gameplay`.
- Homework/prototype folders under `Assets/_Project/Scripts/Homework`.
- Prototype scenes:
  - `Assets/_Project/Scenes/L4MovementScene.unity`
  - `Assets/_Project/Scenes/L5Teleport.unity`
- Imported asset pack demo folders that are not referenced by active scenes or prefabs.
- Duplicate or old config assets only after confirming they are not referenced by `DefendLevelsConfig`.

## Need Manual Verification

- Unity Build Settings scene list.
- References from `BootstrapScene`, `MainMenuScene`, and `GameplayScene`.
- `Resources` paths used by `ConfigService`, `ViewsFactory`, and gameplay factories.
- Prefab references inside active config assets.
- Any imported materials, animations, VFX, or SFX that active prefabs depend on.

## Do Not Remove Yet

- `Assets/_Project/Scripts/DefendGame`.
- `Assets/_Project/Scripts/Infrastructure`.
- `Assets/_Project/Scripts/Progress`.
- Shared UI core under `Assets/_Project/Scripts/UI/Core`, `Currency`, and `Popups`.
- `Assets/_Project/Resources/Configs/DefendGame`.
- `Assets/_Project/Resources/UI`.
- `BootstrapScene`, `MainMenuScene`, and `GameplayScene`.

## Risks

- Unity serialized references can break if assets, prefabs, scripts, or `.meta` files move.
- `Resources.Load` paths are string-based and will fail at runtime if assets move.
- Imported asset packs may contain shared materials or scripts used by active prefabs.
- Removing old scenes before checking Build Settings can hide scene-flow regressions.
- Removing tracked generated files should be done with `git rm --cached`, not filesystem deletion.
