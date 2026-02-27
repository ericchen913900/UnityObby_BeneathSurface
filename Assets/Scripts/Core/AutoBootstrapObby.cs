using BeneathSurface.Obstacles;
using BeneathSurface.Player;
using BeneathSurface.Stage;
using UnityEngine;

namespace BeneathSurface.Core
{
    public static class AutoBootstrapObby
    {
        private const string RootName = "BeneathSurface_Runtime";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void BuildIfMissing()
        {
            if (Object.FindObjectOfType<ObbyRunController>() != null)
            {
                return;
            }

            var root = new GameObject(RootName);
            var spawn = new GameObject("SpawnPoint");
            spawn.transform.SetParent(root.transform);
            spawn.transform.position = new Vector3(0f, 2f, 0f);

            var player = CreatePlayer(root.transform, spawn.transform.position);
            var runController = root.AddComponent<ObbyRunController>();
            runController.Configure(spawn.transform, player.GetComponent<RespawnablePlayer>());

            CreateCamera(root.transform, player.transform);
            CreateBaseFloor(root.transform);
            CreateSimpleCourse(root.transform, runController);
            CreateThemeController(root.transform, runController);
        }

        private static GameObject CreatePlayer(Transform parent, Vector3 spawnPosition)
        {
            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.transform.SetParent(parent);
            player.transform.position = spawnPosition;
            player.AddComponent<CharacterController>();
            player.AddComponent<PlayerMotor>();
            player.AddComponent<RespawnablePlayer>();
            return player;
        }

        private static void CreateCamera(Transform parent, Transform target)
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                var camObj = new GameObject("Main Camera");
                cam = camObj.AddComponent<Camera>();
                cam.tag = "MainCamera";
            }

            cam.transform.SetParent(parent);
            cam.transform.position = new Vector3(0f, 6f, -8f);
            var follow = cam.gameObject.GetComponent<FollowCamera>();
            if (follow == null)
            {
                follow = cam.gameObject.AddComponent<FollowCamera>();
            }

            follow.Configure(target);
        }

        private static void CreateBaseFloor(Transform parent)
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "BaseFloor";
            floor.transform.SetParent(parent);
            floor.transform.position = new Vector3(0f, -1.5f, 55f);
            floor.transform.localScale = new Vector3(16f, 1f, 120f);
        }

        private static void CreateSimpleCourse(Transform parent, ObbyRunController runController)
        {
            for (var i = 0; i < 10; i++)
            {
                var z = i * 11f;
                var y = -i * 0.35f;

                var platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
                platform.name = $"Platform_{i + 1:00}";
                platform.transform.SetParent(parent);
                platform.transform.position = new Vector3(0f, y, z + 4f);
                platform.transform.localScale = new Vector3(6f, 1f, 8f);

                if (i % 2 == 1)
                {
                    var mover = platform.AddComponent<MovingPlatform>();
                    mover.enabled = true;
                }

                var checkpointObj = new GameObject($"Checkpoint_{i + 1:00}");
                checkpointObj.transform.SetParent(parent);
                checkpointObj.transform.position = new Vector3(0f, y + 1.2f, z + 7f);
                var cpCol = checkpointObj.AddComponent<BoxCollider>();
                cpCol.isTrigger = true;
                cpCol.size = new Vector3(4f, 2f, 1.2f);
                var cp = checkpointObj.AddComponent<CheckpointTrigger>();
                cp.Configure(i + 1, runController);

                if (i % 3 == 2)
                {
                    var laser = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    laser.name = $"Laser_{i + 1:00}";
                    laser.transform.SetParent(parent);
                    laser.transform.position = new Vector3(0f, y + 1f, z + 2f);
                    laser.transform.localScale = new Vector3(5f, 0.35f, 0.5f);

                    var laserCollider = laser.GetComponent<BoxCollider>();
                    laserCollider.isTrigger = true;
                    var laserHazard = laser.AddComponent<TimedLaserHazard>();
                    laserHazard.Configure(runController, laser.GetComponent<Renderer>());
                }

                var pit = new GameObject($"PitTrigger_{i + 1:00}");
                pit.transform.SetParent(parent);
                pit.transform.position = new Vector3(0f, y - 2f, z + 4f);
                var pitCol = pit.AddComponent<BoxCollider>();
                pitCol.isTrigger = true;
                pitCol.size = new Vector3(8f, 1f, 10f);
                var pitKill = pit.AddComponent<KillOnTouch>();
                pitKill.Configure(runController);
            }
        }

        private static void CreateThemeController(Transform parent, ObbyRunController runController)
        {
            var themeObj = new GameObject("ThemeController");
            themeObj.transform.SetParent(parent);
            var controller = themeObj.AddComponent<StageThemeController>();

            var profiles = new[]
            {
                new StageThemeController.DepthProfile
                {
                    minCheckpoint = 0,
                    ambientLight = new Color(0.62f, 0.66f, 0.7f, 1f),
                    fogColor = new Color(0.75f, 0.78f, 0.82f, 1f),
                    fogDensity = 0.003f
                },
                new StageThemeController.DepthProfile
                {
                    minCheckpoint = 4,
                    ambientLight = new Color(0.45f, 0.42f, 0.38f, 1f),
                    fogColor = new Color(0.42f, 0.37f, 0.31f, 1f),
                    fogDensity = 0.01f
                },
                new StageThemeController.DepthProfile
                {
                    minCheckpoint = 8,
                    ambientLight = new Color(0.18f, 0.2f, 0.24f, 1f),
                    fogColor = new Color(0.1f, 0.12f, 0.16f, 1f),
                    fogDensity = 0.022f
                }
            };

            controller.Configure(runController, profiles);
        }
    }
}
