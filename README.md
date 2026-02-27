# Beneath the Surface - Unity WebGL Obby

Theme: "Beneath the Surface" (Chinese: 表面之下)

This is a modular obby scaffold for Unity (WebGL-first) with:
- 10-stage progression support via segment definitions
- checkpoint and instant respawn loop
- obstacle scripts (moving platform, touch kill, timed laser)
- depth-based atmosphere controller for the theme
- industrial visual director pass and HUD progress UI

## Unity Version

- Recommended: Unity 2022.3 LTS or newer

## Quick Setup (Instant Play)

1. Create a new 3D Unity project (2022.3 LTS+ recommended).
2. Copy this folder's `Assets/` into your Unity project root.
3. Open any scene (even an empty scene).
4. Press Play.

`AutoBootstrapObby` will auto-generate a playable course (player, camera, checkpoints, hazards, depth theme) if no `ObbyRunController` exists.

## Controls

- Move: WASD
- Jump: Space
- Goal: pass checkpoints and reach deeper sections

Detailed open-source asset references are in `docs/OpenSourceAssets.md`.

## Pull Open-Source Assets

```bash
bash scripts/pull_open_assets.sh
```
