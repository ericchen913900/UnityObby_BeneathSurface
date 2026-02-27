# Beneath the Surface Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Deliver a playable Unity WebGL obby scaffold with 10-stage modular progression, checkpoints, and depth-based atmosphere.

**Architecture:** Runtime stage composition uses reusable segment prefabs. Core gameplay state is checkpoint-driven, with hazards calling a centralized run controller. Visual tone shifts by checkpoint threshold to reinforce the theme.

**Tech Stack:** Unity C# scripts, Unity Test Framework (EditMode), WebGL build target.

---

### Task 1: Core checkpoint state

**Files:**
- Modify: `Assets/Scripts/Core/CheckpointService.cs`
- Test: `Assets/Tests/EditMode/CheckpointServiceTests.cs`

**Step 1: Write failing test**
- Add a test for rejecting non-increasing checkpoint indices.

**Step 2: Run test to verify it fails**
- Run: Unity Test Runner (EditMode) for `CheckpointServiceTests`.

**Step 3: Write minimal implementation**
- Ensure `TrySetCheckpoint` only accepts increasing indices.

**Step 4: Run test to verify it passes**
- Re-run Unity Test Runner (EditMode).

### Task 2: Player fail/respawn loop

**Files:**
- Modify: `Assets/Scripts/Core/ObbyRunController.cs`
- Modify: `Assets/Scripts/Player/RespawnablePlayer.cs`

**Step 1: Wire player reference and spawn flow**
- Ensure `KillPlayer` always respawns from current checkpoint.

**Step 2: Verify in Play Mode**
- Trigger hazard and confirm instant respawn.

### Task 3: Modular stage assembly

**Files:**
- Modify: `Assets/Scripts/Stage/StageBuilder.cs`
- Modify: `Assets/Scripts/Stage/StageSegmentDefinition.cs`

**Step 1: Create segment definitions**
- Define at least 3 segments and assign prefabs.

**Step 2: Build 10-stage run**
- Use `Build Stage` context menu and verify path continuity.

### Task 4: Theme depth progression

**Files:**
- Modify: `Assets/Scripts/Stage/StageThemeController.cs`

**Step 1: Configure depth profiles**
- Surface, mid, and deep profile values.

**Step 2: Verify runtime switches**
- Reach checkpoints and confirm fog/light transitions.

### Task 5: Documentation and licensing

**Files:**
- Modify: `docs/OpenSourceAssets.md`
- Modify: `docs/SceneSetup.md`
- Modify: `docs/Attribution.example.md`

**Step 1: Confirm all imported assets have compatible licenses**
- Keep only CC0 / equivalent / CC BY with attribution records.

**Step 2: Update final attribution entries**
- Add exact title, author, URL, license for each external asset.
