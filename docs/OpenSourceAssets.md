# Open-Source / Permissive Asset Sources

Project theme: Beneath the Surface (表面之下)
Target: Unity WebGL obby

Use only assets whose individual files match the listed license terms.

## Quick Pull

From the repository root:

```bash
bash scripts/pull_open_assets.sh
```

Downloaded archives are saved into `Assets/OpenSource/Raw`.

## Direct Download Set Used By This Project

1. Kenney Prototype Kit (CC0)
   - URL: https://opengameart.org/sites/default/files/kenney_prototype-kit.zip
2. Kenney Prototype Textures (CC0)
   - URL: https://opengameart.org/sites/default/files/kenney_prototypeTextures.zip
3. 100 CC0 SFX by rubberduck (CC0)
   - URL: https://opengameart.org/sites/default/files/100-CC0-SFX_0.zip
4. Royalty Free Game Loops by Pudgyplatypus (CC0)
   - URL: https://opengameart.org/sites/default/files/GameLoops.zip
5. 3xBlast Free Music Pack (CC0)
   - URL: https://opengameart.org/sites/default/files/3xBlast%20-%20Free%20Music%20Pack.zip

## Recommended Sources

1. Kenney (Prototype + Platformer Kits)
   - Type: 3D props, blocks, obstacles, UI
   - License: CC0 1.0
   - Attribution: Not required
   - URL: https://kenney.nl/assets
   - Unity note: Import models/textures into `Assets/Art/Kenney/` and create shared materials.

2. Poly Haven
   - Type: PBR textures and HDRIs
   - License: CC0
   - Attribution: Not required
   - URL: https://polyhaven.com/
   - Unity note: Use textures for rock/metal surfaces; compress textures for WebGL.

3. ambientCG
   - Type: PBR materials (rock, dirt, concrete)
   - License: CC0
   - Attribution: Not required
   - URL: https://ambientcg.com/
   - Unity note: Build tileable materials for stage segments.

4. Quaternius
   - Type: Low-poly environment packs
   - License: CC0
   - Attribution: Not required
   - URL: https://quaternius.com/
   - Unity note: Great for modular obstacle prefabs.

5. OpenGameArt (filter to CC0 only)
   - Type: Mixed 2D/3D + audio
   - License: Varies; use CC0 entries only
   - Attribution: Not required for CC0 assets
   - URL: https://opengameart.org/
   - Unity note: Confirm each asset page license before import.

6. Freesound (filter to CC0)
   - Type: SFX (impact, machinery, alarm)
   - License: Varies; use CC0 entries only
   - Attribution: Not required for CC0 assets
   - URL: https://freesound.org/
   - Unity note: Normalize loudness and convert to OGG for WebGL size control.

7. Incompetech (Kevin MacLeod)
   - Type: Background music
   - License: CC BY 4.0
   - Attribution: Required
   - URL: https://incompetech.com/music/
   - Unity note: Keep one looping BGM per depth zone to reduce memory.

8. Openverse
   - Type: License-discoverable audio/image assets
   - License: Mixed; choose CC0/CC BY only
   - Attribution: Required for CC BY
   - URL: https://openverse.org/
   - Unity note: Store proof of license and author in project docs.

## Attribution Template

For CC BY assets, include this in your in-game credits or README:

- Title: <asset title>
- Author: <author name>
- Source: <URL>
- License: <license name + URL>
