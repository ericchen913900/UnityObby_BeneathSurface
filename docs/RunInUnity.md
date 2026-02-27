# Run in Unity (Playable Immediately)

## Steps

1. Open Unity Hub and create a new 2D project (Unity 2022.3 LTS+).
2. Copy `UnityObby_BeneathSurface/Assets` into your Unity project root.
3. Open any scene (an empty scene is fine).
4. Click Play.

If there is no `ObbyRunController` in the scene, `AutoBootstrapObby` will automatically create:
- player + camera
- 10 course segments
- checkpoints and kill hazards
- biome atmosphere changes (Glow Caves -> Vertical Ruins -> Crust Fault)
- patterned sprite visual pass
- HUD with progress, biome, and story status

## Controls

- `A/D` or `Left/Right`: move
- `Space`: jump

## Optional Open-Source Asset Pull

```bash
bash scripts/pull_open_assets.sh
```

Then unpack selected archives from `Assets/OpenSource/Raw` into your Unity `Assets` subfolders.

## Troubleshooting

- If nothing auto-spawns, check Console for compile errors.
- If player does not move, set `Project Settings -> Player -> Active Input Handling` to `Both` and restart Unity.
- If auto-bootstrap does not run because your scene already has `ObbyRunController`, remove it and press Play again.
