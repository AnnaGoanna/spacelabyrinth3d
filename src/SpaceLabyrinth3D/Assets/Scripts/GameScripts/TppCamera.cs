using UnityEngine;

namespace GameScripts
{
    public class TppCamera : MonoBehaviour
    {
        private float _mouseX;
        private float _mouseY;
        private float _rotation;
        private float _zoom;
        private Vector3 _initialPosition;

        public GameObject Player;

        private void Start()
        {
            _initialPosition = transform.localPosition;
        }

        private void Update()
        {
            _mouseX += Input.GetAxis("Mouse X");
            _mouseY += Input.GetAxis("Mouse Y");

            transform.parent.transform.position = Player.transform.position;

            if (Input.GetButton("ToggleZoomRotation"))
            {
                _rotation += Input.GetAxis("Mouse ScrollWheel");
            }
            else
            {
                _zoom += Input.GetAxis("Mouse ScrollWheel");

                if (_zoom > 0)
                {
                    _zoom = 0;
                }
                transform.localPosition =
                    new Vector3(0, _initialPosition.y - _zoom * 5, _initialPosition.z + _zoom * 5);
            }

            transform.eulerAngles = new Vector3(-_mouseY * 10, _mouseX * 10, _rotation * 50);
        }
    }
}
