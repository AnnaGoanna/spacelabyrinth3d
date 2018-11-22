using UnityEngine;

namespace GameScripts
{
    public class CameraToggle : MonoBehaviour
    {
        public Camera FppCamera;
        public Camera TppCamera;
        public Camera CrosshairCamera;

        private void Start()
        {
            TppCamera.enabled = false;
            FppCamera.enabled = true;
            CrosshairCamera.enabled = true;
        }

        private void Update()
        {
            if (Input.GetButtonDown("ToggleCamera"))
            {
                TppCamera.enabled = !TppCamera.enabled;
                FppCamera.enabled = !FppCamera.enabled;
                CrosshairCamera.enabled = !CrosshairCamera.enabled;
            }
        }
    }
}
