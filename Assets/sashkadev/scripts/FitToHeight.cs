using UnityEngine;
namespace sashkadev.game
{
    public class FitToHeight : MonoBehaviour
    {
        [SerializeField]
        LayerMask fitMask;

        float targetHeight;
        [SerializeField]
        Vector3 localRayOrigin = new(0,10f,0);
        void Awake()
        {
            targetHeight = transform.localPosition.y;
        }

        void Update()
        {
            Vector3 origin = localRayOrigin + transform.position;

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 60f, fitMask))
            {
                Vector3 tP = hit.point;
                tP.y += targetHeight;
                transform.localPosition = new (transform.localPosition.x, tP.y, transform.localPosition.z);
            }
        }
    }
}

