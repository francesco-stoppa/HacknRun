using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField] private float mouseSensitivity = 0.15f;

    [Header("Rotation Limits")]
    [SerializeField] private bool robot = true;

    [SerializeField] private float minPitch = -60f;
    [SerializeField] private float maxPitch = 60f;

    [SerializeField] private float minYaw = -45f;
    [SerializeField] private float maxYaw = 45f;

    [Header("Axis Lock")]
    [SerializeField] private bool rumba = false;

    private float pitch;
    private float yaw;

    private void Start()
    {
        Vector3 angles = transform.localEulerAngles;

        pitch = NormalizeAngle(angles.x);
        yaw = NormalizeAngle(angles.y);
    }

    private void Update()
    {
        Vector2 mouse = Mouse.current.delta.ReadValue();

        // Destra/Sinistra
        yaw += mouse.x * mouseSensitivity;

        // Su/Giù
        if (!rumba)
            pitch -= mouse.y * mouseSensitivity;

        if (!robot)
        {
            // Il limite sul pitch rimane sempre
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            // Il limite sullo yaw rimane sempre
            yaw = Mathf.Clamp(yaw, minYaw, maxYaw);
        }

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }


    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;

        return angle;
    }
}


