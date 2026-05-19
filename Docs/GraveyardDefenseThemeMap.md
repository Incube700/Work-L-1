# Graveyard Defense Theme Map

This is a visual and presentation rename pass for the existing Tower Defence prototype. Gameplay rules stay in the current Defend systems.

| Current concept | Graveyard Defense concept | Conversion note |
| --- | --- | --- |
| `DummyEnemy` | Zombie | Keep the same melee enemy logic. Swap the prefab/config visual to a zombie wrapper. |
| `ShooterEnemy` | Ranged undead / skeleton archer / spitter | Keep the same ranged enemy logic. Swap visual and projectile presentation only. |
| Mine | Magic Rune / explosive trap | Keep `MineConfig` and mine placement behavior. Rename in UI via `DefendUiIconConfig`. |
| Turret | Magic Totem / crystal tower | Keep `TurretConfig`, target scan, and projectile shooting behavior. Swap visual wrapper and projectile VFX. |
| Puddle | Curse / poison swamp / slow curse | Keep `PuddleConfig` and puddle effect behavior. Swap visual wrapper and UI label. |
| Base | Ritual Stone / wizard tower | Keep `BuildingConfig` and base health/damage behavior. Swap visual wrapper if local theme is enabled. |
| Gold | Souls or Gold | Prefer `Souls` for theme flavor; keep `CurrencyType.Gold` in code. |
| Diamond | Crystals | Keep `CurrencyType.Diamond` in code and rename via presentation config. |
| Wave | Horde / Night | Keep wave config and state machine names in code. Use `Horde`/`Night` in UI copy where presentation config supports it. |
| Rest phase | Build / Prepare | Keep `DefendPhase.Rest`. Display as `Prepare`. |
| Wave phase | Defend | Keep `DefendPhase.Wave`. Display as `Defend`. |
| Result win | Survived | Popup copy can say the player survived the horde. |
| Result loss | Fallen | Popup copy can say the ritual stone fell. |

## First Playable Labels

Use these names for the first local pass:

| UI surface | Current label | First themed label |
| --- | --- | --- |
| Placement button | Mine | Rune |
| Placement button | Turret | Totem |
| Placement button | Puddle | Curse |
| Currency row | Gold | Souls |
| Currency row | Diamond | Crystals |
| Phase text | Rest | Prepare |
| Phase text | Wave | Defend |
| Wave text | Wave | Horde |

Do not rename enums, config class names, serialized field names, factories, or services for this pass. That keeps existing scenes, prefab GUIDs, and save/config behavior stable.
