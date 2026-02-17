using Unity.Cinemachine;
using UnityEngine;

namespace sashkadev.game
{
    public class CinemachinePrioritySwitcher : MonoBehaviour
    {
        public CinemachineVirtualCameraBase topdownCam;
        public CinemachineVirtualCameraBase overviewCam; 

        public int activePriority = 1;
        public int inactivePriority = -1;

        public bool isOverviewActive;

        void Start()
        {
            Switch();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isOverviewActive = !isOverviewActive;
                Switch();
            }
        }

        void Switch()
        {
            if (!topdownCam || !overviewCam) return;

            if (isOverviewActive)
            {
                overviewCam.Priority = activePriority;
                topdownCam.Priority = inactivePriority;
            }
            else
            {
                topdownCam.Priority = activePriority;
                overviewCam.Priority = inactivePriority;
            }
        }
    }

}
