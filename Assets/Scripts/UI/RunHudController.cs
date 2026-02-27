using BeneathSurface.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BeneathSurface.UI
{
    public class RunHudController : MonoBehaviour
    {
        [SerializeField] private ObbyRunController runController;
        [SerializeField] private Text checkpointText;
        [SerializeField] private Text statusText;
        [SerializeField] private Image progressFill;

        private void Update()
        {
            if (runController == null)
            {
                runController = FindObjectOfType<ObbyRunController>();
            }

            if (runController == null)
            {
                return;
            }

            var checkpoint = runController.GetCurrentCheckpointIndex();
            var finish = runController.GetFinishCheckpoint();
            var progress = runController.GetProgress01();

            if (checkpointText != null)
            {
                checkpointText.text = "Depth Progress " + checkpoint + " / " + finish;
            }

            if (statusText != null)
            {
                statusText.text = runController.HasFinished() ? "Core Reached - Run Complete" : "Descend Beneath The Surface";
            }

            if (progressFill != null)
            {
                progressFill.fillAmount = progress;
            }
        }

        public void Configure(ObbyRunController controller, Text checkpoint, Text status, Image fill)
        {
            runController = controller;
            checkpointText = checkpoint;
            statusText = status;
            progressFill = fill;
        }
    }
}
