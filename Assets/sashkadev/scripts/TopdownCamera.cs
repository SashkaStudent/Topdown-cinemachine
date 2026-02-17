using Unity.Cinemachine;
using UnityEngine;

namespace sashkadev.game
{
    public class TopdownCamera : MonoBehaviour
    {
        [Header("Mixing camera")]
        public CinemachineMixingCamera mixCam;

        [Header("Move")]
        [SerializeField]
        private float moveSpeed = 100f, moveSmoothTime = 0.1f;

        [Header("Zoom")]
        [SerializeField, Range(0f, 1f)]
        private float t = 1f;

        [SerializeField]
        private float scrollSmoothTime = 0.1f;
        [SerializeField]
        private AnimationCurve scrollSpeedByZoom, moveSpeedByZoom;

        [Header("Rotate")]
        [SerializeField]
        private float rotateSpeed = 180f, rotateSmooth = 0.1f;

        Vector3 panDampVelocity;
        Vector3 targetPos;
        float smoothDampVelocity;
        float targetT;
        float targetYaw;
        float yaw;
        float yawDampVelocity;

        void Reset()
        {
            mixCam = GetComponent<CinemachineMixingCamera>();

        }
        void Start()
        {
            targetT = t;
            targetPos = transform.position;
            ApplyWeights(t);
        }
        void Update()
        {
            if (!mixCam) return;

            HandleScroll();
            HandleOrbit();
            HandleMove();
            ApplyWeights(t);

        }

        void HandleScroll()
        {
            float scroll = Input.mouseScrollDelta.y * scrollSpeedByZoom.Evaluate(targetT);
            targetT = Mathf.Clamp01(targetT + scroll * 0.1f);
            t = Mathf.SmoothDamp(t, targetT, ref smoothDampVelocity, scrollSmoothTime);
        }
        void HandleOrbit()
        {
            if (Input.GetMouseButton(2))
            {
                float mouseX = Input.GetAxisRaw("Mouse X");
                targetYaw += mouseX * rotateSpeed * Time.deltaTime;
            }
            yaw = Mathf.SmoothDampAngle(yaw, targetYaw, ref yawDampVelocity, rotateSmooth);
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        }

        void HandleMove()
        {
            float dx = Input.GetAxis("Horizontal");
            float dy = Input.GetAxis("Vertical");

            Vector3 right = transform.right;
            Vector3 forward = transform.forward;

            right.y = 0f;
            right.Normalize();
            forward.y = 0f;
            forward.Normalize();

            Vector3 deltaWorld = (right * dx + forward * dy) * moveSpeed * moveSpeedByZoom.Evaluate(targetT) * Time.deltaTime;
            targetPos += deltaWorld;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref panDampVelocity, moveSmoothTime);

        }
        void ApplyWeights(float tt)
        {
            int n = mixCam.ChildCameras.Count;
            if (n == 0) return;

            if (n == 1)
            {
                mixCam.SetWeight(0, 1f);
                return;
            }

            float pos = Mathf.Clamp01(tt) * (n - 1);
            int i = Mathf.FloorToInt(pos);
            int j = Mathf.Min(i + 1, n - 1);
            float u = pos - i;

            for (int k = 0; k < n; k++)
                mixCam.SetWeight(k, 0f);

            mixCam.SetWeight(i, 1f - u);
            mixCam.SetWeight(j, u);
        }
    }


}

