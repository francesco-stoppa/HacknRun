using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField] private float mouseSensitivity = 0.15f;

    bool robot = false;
    bool rumba = false;

    [Header("Rotation Limits")]

    [SerializeField] private float minPitch = -60f;
    [SerializeField] private float maxPitch = 60f;

    [SerializeField] private float minYaw = -45f;
    [SerializeField] private float maxYaw = 45f;


    private float pitch;
    private float yaw;


    // ADD
    bool active = false;
    [Tooltip("Da assegnare solo al primo robot di cui h ail controllo il Player")]
    public Canvas canvas;

    private void Start()
    {

        Vector3 angles = transform.localEulerAngles;

        pitch = NormalizeAngle(angles.x);
        yaw = NormalizeAngle(angles.y);

    }

    public void ActiveCameraController(bool robot = false, bool rumba = false)
    {

        SetCamera(robot, rumba);

        active = true;
    }

    public void SetCamera(bool robot = false, bool rumba = false)
    {
        this.robot = robot;
        this.rumba = rumba;
    }


    private void Update()
    {
        if (!active) return;


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


