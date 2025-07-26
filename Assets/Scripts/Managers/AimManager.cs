using UnityEngine;

namespace Managers
{
    public class AimManager : MonoBehaviour
    {
        [SerializeField] private Transform aimTransform;
        [SerializeField] private GameObject crosshair;
        [SerializeField] private float controllerCrosshairDistance = 5;
        private AimMode _activeAimMode;
    
        public enum AimMode
        {
            Stick,
            Mouse
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (_activeAimMode == AimMode.Mouse)
            {
                UpdateCrosshair(Input.mousePosition, AimMode.Mouse);
                UpdateCamera(Input.mousePosition, AimMode.Mouse);
            }
        }

        public void Aim(Vector2 vector, AimMode mode)
        {
            _activeAimMode = mode;
            if (aimTransform)
            {
                if (mode == AimMode.Mouse && CameraFollow2D.Instance.Camera)
                {
                    var screenPoint = (Vector2)CameraFollow2D.Instance.Camera.WorldToScreenPoint(transform.position);
                    aimTransform.right = vector - screenPoint;
                }
                else
                {
                    aimTransform.right = vector;
                }
            }

            UpdateCrosshair(vector, mode);
            UpdateCamera(vector, mode);
        }

        private void UpdateCamera(Vector2 vector, AimMode mode)
        {
            if (CameraFollow2D.Instance.Target == gameObject)
            {
                var screenToWorldPoint = GetWorldAimPoint(vector, mode);
                CameraFollow2D.Instance.Offset = (screenToWorldPoint - transform.position) / 6;
            }
        }

        private void UpdateCrosshair(Vector2 vector, AimMode mode)
        {
            var screenToWorldPoint = GetWorldAimPoint(vector, mode);
            if (crosshair)
            {
                crosshair.transform.position = screenToWorldPoint;
            }
        }

        private Vector3 GetWorldAimPoint(Vector2 vector, AimMode mode)
        {
            Vector3 screenToWorldPoint = transform.position;
            if (mode == AimMode.Mouse && CameraFollow2D.Instance.Camera)
            {
                var screenPointToRay = CameraFollow2D.Instance.Camera.ScreenPointToRay(vector);
                Plane plane = new Plane(-Vector3.forward, transform.position);
                float enter;
                if (plane.Raycast(screenPointToRay, out enter))
                {
                    screenToWorldPoint = screenPointToRay.GetPoint(enter);
                }
            }
            else
            {
                screenToWorldPoint = transform.position + (Vector3)vector * controllerCrosshairDistance;
            }
            screenToWorldPoint.z = -9;
            return screenToWorldPoint;
        }

        public Vector3 GetAimDirection()
        {
            return aimTransform.right;
        }

        public void EnableCrosshair()
        {
            if (crosshair)
            {
                crosshair.SetActive(true);
            }
        }

        public void DisableCrosshair()
        {
            if (crosshair)
            {
                crosshair.SetActive(false);
            }
        }
    }
}
