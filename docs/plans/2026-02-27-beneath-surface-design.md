# Beneath the Surface - Design

## Goal

Build a Unity WebGL obby where gameplay and visual atmosphere descend from a safe surface into deeper, more dangerous layers.

## Core Loop

1. Traverse obstacle segment.
2. Reach checkpoint.
3. Fail on hazards and instantly respawn at latest checkpoint.
4. Progress deeper; atmosphere changes by checkpoint tier.

## Systems

- Checkpoints: monotonic checkpoint index progression.
- Respawn: fast reset to latest spawn point.
- Stage modules: reusable segment prefabs repeated to create a 10-stage run.
- Theme depth: fog/ambient profile swap by checkpoint depth.

## WebGL Constraints

- Keep geometry and textures light.
- Prefer baked/static visuals where possible.
- Use short looping audio and compressed formats.

## Asset Policy

- Prioritize CC0/public-domain assets.
- For CC BY assets, keep attribution records in docs and in-game credits.
