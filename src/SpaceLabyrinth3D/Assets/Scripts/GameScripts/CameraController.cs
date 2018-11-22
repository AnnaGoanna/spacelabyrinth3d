using GameScripts;
using UnityEngine;
using UnityEngine.VR;

public class CameraController : MonoBehaviour
{
    public Camera FppCamera;
    public Camera TppCamera;
    public Camera UiCamera;
    public Camera CrosshairCamera;

    private float _rotationX;
    private float _rotationY;

    private void Start()
    {
        UnityEngine.XR.InputTracking.disablePositionalTracking = true;
        DisableTrackingOnCameras();

        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            Debug.Log("Enabling gyroscope input.");
        }
    }

    private void DisableTrackingOnCameras()
    {
        var camerasWithDisabledRotationTracking = new[] {UiCamera, CrosshairCamera};
        foreach (var notTrackedCamera in camerasWithDisabledRotationTracking)
        {
            // Uncomment on Unity 2017
            // VRDevice.DisableAutoVRCameraTracking(notTrackedCamera, true);
        }
    }

    private void Update()
    {
        // Disable if above Unity 2017 method is used
        ApplyOffsetOnCameras();

        if (Input.gyro.enabled)
        {
            const float scale = 1.5f;
            _rotationX += -Input.gyro.rotationRateUnbiased.x * scale;
            _rotationY += -Input.gyro.rotationRateUnbiased.y * scale;
            FppCamera.transform.localEulerAngles = new Vector3(_rotationX, _rotationY, 0);
        }
    }

    private void ApplyOffsetOnCameras()
    {
        var camerasWithDisabledRotationTracking = new[] {UiCamera, CrosshairCamera};

        foreach (var notTrackedCamera in camerasWithDisabledRotationTracking)
        {
            notTrackedCamera.transform.localRotation =
                Quaternion.Inverse(UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye));
        }
    }
}
