# Theme Conversion Risks

## Paid And Vendor Asset Risk

Treat these folders as local-only until their licenses are confirmed:

| Folder | Risk | Rule |
| --- | --- | --- |
| `Assets/Toon_Zombies_extended/` | Imported zombie character pack may be paid or non-redistributable. | Do not commit source assets. Do not copy into tracked project folders. |
| `Assets/MasterMagicFX/` | Imported magic VFX pack may be paid or non-redistributable. | Do not commit source assets. Keep references local-only. |
| `Assets/Artsystack - Fantasy RPG GUI/` | Paid UI pack. | Keep ignored. Use only through ignored generated UI unless a private repo policy is chosen. |
| `Assets/_LocalGenerated/` | Generated assets may reference paid/local packs. | Keep ignored and do not commit. |

Public GitHub clones will not have these assets. Any tracked scene, prefab, or config that references their GUIDs can produce missing references for other users.

## Public Repository Risk

- Do not commit vendor source files, textures, meshes, prefabs, particles, audio, or `.meta` files from local-only packs.
- Do not commit tracked scenes or prefabs after assigning local-only generated skins unless missing references are explicitly accepted.
- Do not commit tracked configs that point `PrefabPath` to ignored Resources folders; public clones will load nothing at runtime.
- Do not commit `ProjectSettings/PackageManagerSettings.asset` or `UserSettings/EditorUserSettings.asset` unless the change is reviewed and intentional.

## Missing Reference Risk

Generated local prefabs can reference:

- Zombie prefabs from `Toon_Zombies_extended`.
- VFX prefabs/materials from `MasterMagicFX`.
- Graveyard props from imported environment folders.
- Artsystack sprites/fonts through the UI builder.

If those packs are absent, generated local prefabs may show fallback primitives or missing references. Keep the old tracked dummy prefabs/configs intact so the base game still works.

## Render Pipeline And Material Risk

- Imported assets may use shaders that do not match the active render pipeline.
- Toon character materials may need shader upgrades or manual material reassignment.
- Particle materials from VFX packs may render pink, too bright, too large, or incorrectly sorted.
- UI sprites from Artsystack may need slicing, borders, or Canvas scaling adjustments.

Check materials in Unity before using any imported asset in a tracked prefab.

## Animation And Controller Risk

- Zombie FBX/prefab animation controllers may not match the existing `EnemyAnimatorView` expectations.
- The local builder tries to assign a child `Animator` to the copied enemy view when one exists.
- Vendor animation scripts are stripped from generated visual children so gameplay stays owned by Defend systems.
- If animation clips do not play, fix the visual child Animator/controller locally before promoting anything to tracked assets.

## Collider, Scale, And Registration Risk

- The original Defend prefabs contain the colliders and `MonoEntity` lifecycle expected by factories.
- Vendor visual child colliders and rigidbodies are stripped by the local builder to avoid changing gameplay collisions.
- Visual scale may not match gameplay radius, especially for tank zombies, grave markers, and large VFX.
- Validate that click placement, enemy overlap, projectile hit distance, and base damage still work after visual replacement.

## Physics And Pathing Risk

The current Defend gameplay appears to use direct runtime movement and collision/overlap checks rather than a full NavMesh conversion. Still verify:

- Enemy visuals do not rotate or scale in a way that hides their movement direction.
- Large visual children do not imply a different collision radius than the runtime config.
- Graveyard arena props do not block placement or enemy movement unless gameplay code explicitly supports obstacles.
- Particle effects do not include colliders or scripts that interfere with overlap checks.

## Scene Serialization Risk

- Assigning local generated prefabs into tracked scenes serializes GUID references.
- Those GUIDs resolve only on the local machine because `_LocalGenerated` is ignored.
- Keep scene assignments local, or revert scene changes before committing.
- Prefer generating local wrappers/configs again through the Editor menu instead of committing generated asset files.

## What Not To Touch Yet

- Do not rename Defend classes, enums, or serialized fields.
- Do not rewrite the state machine.
- Do not rewrite placement, wave spawning, combat, economy, save/load, or projectile systems.
- Do not modify vendor prefabs directly.
- Do not move/delete legacy assets.
- Do not break `.meta` GUIDs.
- Do not stage or commit local-only generated/vendor assets.
