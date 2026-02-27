using BeneathSurface.Core;
using UnityEngine;

namespace BeneathSurface.UI
{
    public class RunHudController : MonoBehaviour
    {
        [SerializeField] private ObbyRunController runController;
        [SerializeField] private bool showHud = true;

        private GUIStyle _checkpointStyle;
        private GUIStyle _statusStyle;
        private GUIStyle _storyStyle;

        private static readonly Color PanelColor = new Color(0.05f, 0.06f, 0.08f, 0.72f);
        private static readonly Color ProgressBackColor = new Color(0.18f, 0.2f, 0.24f, 0.95f);
        private static readonly Color ProgressFillColor = new Color(0.93f, 0.46f, 0.17f, 0.98f);
        private static readonly Color TextColor = new Color(0.93f, 0.94f, 0.96f, 1f);

        private void Update()
        {
            if (runController == null)
            {
                runController = FindObjectOfType<ObbyRunController>();
            }
        }

        private void OnGUI()
        {
            if (!showHud || runController == null)
            {
                return;
            }

            EnsureStyles();

            var checkpoint = runController.GetCurrentCheckpointIndex();
            var finish = runController.GetFinishCheckpoint();
            var progress = runController.GetProgress01();
            var status = runController.HasFinished() ? "Core Reached - Run Complete" : "Climb Up Before Collapse";
            var story = StorylineService.GetObjectiveLine(checkpoint, finish, runController.HasFinished());

            DrawSolid(new Rect(0f, 0f, Screen.width, 116f), PanelColor);
            GUI.Label(new Rect(20f, 12f, 600f, 28f), "Depth Progress " + checkpoint + " / " + finish, _checkpointStyle);
            GUI.Label(new Rect(20f, 42f, 600f, 24f), status, _statusStyle);
            GUI.Label(new Rect(20f, 66f, Screen.width - 40f, 42f), story, _storyStyle);

            const float barWidth = 280f;
            const float barHeight = 18f;
            var barX = Screen.width - barWidth - 20f;
            var barY = 78f;
            DrawSolid(new Rect(barX, barY, barWidth, barHeight), ProgressBackColor);
            DrawSolid(new Rect(barX, barY, barWidth * Mathf.Clamp01(progress), barHeight), ProgressFillColor);
        }

        private void EnsureStyles()
        {
            if (_checkpointStyle != null && _statusStyle != null && _storyStyle != null)
            {
                return;
            }

            _checkpointStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 20,
                alignment = TextAnchor.UpperLeft
            };
            _checkpointStyle.normal.textColor = TextColor;

            _statusStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                alignment = TextAnchor.UpperLeft
            };
            _statusStyle.normal.textColor = TextColor;

            _storyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 13,
                alignment = TextAnchor.UpperLeft,
                wordWrap = true
            };
            _storyStyle.normal.textColor = TextColor;
        }

        private static void DrawSolid(Rect rect, Color color)
        {
            var previous = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = previous;
        }

        public void Configure(ObbyRunController controller)
        {
            runController = controller;
        }
    }
}
