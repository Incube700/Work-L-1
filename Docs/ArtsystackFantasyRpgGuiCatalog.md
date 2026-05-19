# Artsystack Fantasy RPG GUI Catalog

Local paid asset pack path:

`Assets/Artsystack - Fantasy RPG GUI`

This pack is licensed local reference material for UI polish. Do not copy, move, rename, export, or commit the vendor source assets. Project-owned prefabs/configs may be styled locally in the Unity Editor, but required committed references to this ignored pack should be avoided unless a fallback path is added and documented.

## Pack Structure

- `+Document README+/document.pdf`: vendor documentation.
- `PSD File/`: source design files. Local reference only.
- `Preview Images/`: preview renders for layout review. Local reference only.
- `ResourcesData/Font/`: Kurale and MedievalSharp font files/TMP SDF assets.
- `ResourcesData/Prefabs/`: demo/menu/popup prefabs from the pack.
- `ResourcesData/Sprites/bg/`: fantasy UI backgrounds.
- `ResourcesData/Sprites/colored_icon/`: resource/item icons in 256, 512, and original sizes.
- `ResourcesData/Sprites/components/`: panels, frames, buttons, progress bars, slots, popup parts.
- `ResourcesData/Sprites/controller icons/`: keyboard, mouse, PlayStation, and Xbox prompts.
- `ResourcesData/Sprites/flaticon/`: textured icon set.
- `Scenes/DemoScene.unity`: vendor demo scene.
- `Scripts/`: vendor demo/support scripts.

## Candidate Assets

| Candidate | Asset type | Suggested Tower Defence use | Stay local only | Manual assignment | Safe to reference from tracked project prefabs/configs |
| --- | --- | --- | --- | --- | --- |
| `ResourcesData/Sprites/components/button_01.png` | Sprite | Main menu Play button background | Yes | Yes | Local optional only; do not make a required public prefab depend on it |
| `ResourcesData/Sprites/components/button_02.png` | Sprite | Secondary button background for reset/menu actions | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/button_3.png` | Sprite | Compact HUD/placement button background | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/BlueFrame_bg.png` | Sprite | Main menu title panel or calm HUD panel | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/GreenFrame_bg.png` | Sprite | Build/ready phase panel treatment | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/RedFrame_bg.png` | Sprite | Danger/lose/result panel treatment | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/SmallBlueFrame_bg.png` | Sprite | Small counters such as wave or phase | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/header_box.png` | Sprite | Section header behind "Wave" or "Build" labels | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/panel_name_header.png` | Sprite | Main menu title strip or result title strip | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/InfoBox.png` | Sprite | Tooltip/info panel style for future tower descriptions | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/description_box_01.png` | Sprite | Upgrade description panels | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/pop_up.png` | Sprite | Result popup body/background | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/bg/bg.png` | Sprite | Local-only main menu background test | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/bg/bg_2.png` | Sprite | Local-only alternate menu/gameplay background test | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/heart_bg.png` | Sprite | Base health icon/backplate | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/heart_bg_frame.png` | Sprite | Base health frame | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/heart_fill.png` | Sprite | Base health fill image | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/heart_frame.png` | Sprite | Base health decorative frame | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/progress_bar_bg.png` | Sprite | Health/wave progress bar background | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/progress_bar_top.png` | Sprite | Health/wave progress fill | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/exp_bar_bg.png` | Sprite | Alternate wave/reward progress background | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/exp_bar_top.png` | Sprite | Alternate wave/reward progress fill | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/stats_slider_bg.png` | Sprite | Slider/progress background for stats panels | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/stats_slider_top.png` | Sprite | Slider/progress fill for stats panels | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/coins_frame.png` | Sprite | Currency row backplate | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/coins_frame_2.png` | Sprite | Compact currency display frame | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/ingame_icon_slot.png` | Sprite | Mine/Turret/Puddle button slot | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/ingame_icon_slot_2.png` | Sprite | Alternate placement slot | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/icon_slot_active.png` | Sprite | Selected placeable slot state | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/icon_slot_locked.png` | Sprite | Unaffordable/locked placeable slot state | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/item_slot.png` | Sprite | Tower card or reward item slot | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/item_selected_slot.png` | Sprite | Selected tower card state | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/components/skill_slot.png` | Sprite | Permanent upgrade card slot | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/colored_icon/256/coin_1.png` | Sprite | Gold icon in `DefendUiIconConfig` | Yes | Yes | Local optional only; public clone needs fallback |
| `ResourcesData/Sprites/colored_icon/256/coin_2.png` | Sprite | Alternate gold icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/colored_icon/256/Pouch of Coins.png` | Sprite | Larger gold reward icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/colored_icon/256/gold_bar.png` | Sprite | Premium/reward gold icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/colored_icon/256/crystal_1.png` | Sprite | Diamond icon in `DefendUiIconConfig` | Yes | Yes | Local optional only; public clone needs fallback |
| `ResourcesData/Sprites/colored_icon/256/crystal_2.png` | Sprite | Alternate diamond icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/colored_icon/256/Pouch of crystal.png` | Sprite | Reward diamond icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/colored_icon/256/health_potion .png` | Sprite | Health/recovery reward icon candidate | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/flaticon/textured/btn_coin.png` | Sprite | Compact gold HUD icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/flaticon/textured/btn_crystal_1.png` | Sprite | Compact diamond HUD icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/flaticon/textured/btn_Heart.png` | Sprite | Compact base health icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/flaticon/textured/btn_bomb.png` | Sprite | Mine button icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/flaticon/textured/btn_archery.png` | Sprite | Turret/ranged tower button icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/flaticon/textured/btn_dagger.png` | Sprite | Alternate damage/tower icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/flaticon/textured/btn_axe.png` | Sprite | Alternate tower/attack icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/flaticon/textured/btn_castle.png` | Sprite | Base/defence phase icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/flaticon/textured/btn_flag_1.png` | Sprite | Wave indicator icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/flaticon/textured/btn_check.png` | Sprite | Victory/confirm icon | Yes | Yes | Local optional only |
| `ResourcesData/Sprites/flaticon/textured/btn_caution.png` | Sprite | Defeat/warning/denied icon | Yes | Yes | Local optional only |
| `ResourcesData/Font/Kurale-Regular SDF.asset` | TMP font asset | Local fantasy title/body typography test | Yes | Yes | Local optional only; keep TMP fallback in public prefabs |
| `ResourcesData/Font/MedievalSharp-Regular SDF.asset` | TMP font asset | Local fantasy heading typography test | Yes | Yes | Local optional only; keep TMP fallback in public prefabs |
| `ResourcesData/Prefabs/Title.prefab` | Vendor prefab | Main menu layout reference only | Yes | No direct reuse in tracked scene | Not safe for committed scene dependency |
| `ResourcesData/Prefabs/Gameplay.prefab` | Vendor prefab | HUD composition reference only | Yes | No direct reuse in tracked scene | Not safe for committed scene dependency |
| `ResourcesData/Prefabs/Coin.prefab` | Vendor prefab | Currency display reference only | Yes | No direct reuse in tracked scene | Not safe for committed scene dependency |
| `ResourcesData/Prefabs/Crystal.prefab` | Vendor prefab | Diamond display reference only | Yes | No direct reuse in tracked scene | Not safe for committed scene dependency |
| `ResourcesData/Prefabs/PopUp_Pause.prefab` | Vendor prefab | Popup/window reference for result panel | Yes | No direct reuse in tracked prefab | Not safe for committed scene dependency |
| `ResourcesData/Prefabs/PopUp_Update.prefab` | Vendor prefab | Popup/window reference for result/reward panel | Yes | No direct reuse in tracked prefab | Not safe for committed scene dependency |
| `ResourcesData/Prefabs/GameScene_01.prefab` | Vendor prefab | Full-screen UI composition reference | Yes | No direct reuse in tracked scene | Not safe for committed scene dependency |
| `ResourcesData/Prefabs/GameScene_02.prefab` | Vendor prefab | Full-screen UI composition reference | Yes | No direct reuse in tracked scene | Not safe for committed scene dependency |
| `Scenes/DemoScene.unity` | Vendor scene | Inspect pack setup and sprite usage | Yes | Open read-only/local only | Not safe for committed scene dependency |

## Recommended Local Mapping

- Gold: `coin_1.png`, `Pouch of Coins.png`, or `btn_coin.png`.
- Diamonds: `crystal_1.png`, `Pouch of crystal.png`, or `btn_crystal_1.png`.
- Base health: `heart_fill.png` plus `heart_frame.png`, or `btn_Heart.png`.
- Mine: `btn_bomb.png`.
- Turret: `btn_archery.png` or `btn_castle.png`.
- Puddle: no exact match found; use a local placeholder such as `btn_Quick Time.png`, `btn_flame.png`, or keep the current text button until a project-owned icon is created.
- Wave: `btn_flag_1.png`.
- Build phase: `GreenFrame_bg.png` or `btn_gate.png`.
- Defend phase: `RedFrame_bg.png`, `btn_castle.png`, or `btn_archery.png`.
- Victory: `btn_check.png`.
- Defeat/denied: `btn_caution.png`.

## Manual Unity Assignment Notes

- Assign these assets only in the local Unity Editor while the paid pack is present.
- Do not copy sprites, fonts, prefabs, PSDs, or preview images into `Assets/_Project`.
- Do not modify vendor prefabs or demo scenes directly.
- Do not commit project-owned prefab/scene/config changes that make the public project require this ignored vendor folder unless the reference is optional and the missing-reference behavior has been checked.
