using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 120f;
    public float mouseSensitivity = 0.15f;

    private Rigidbody rb;

    private float pitch = 0f;
    private float yaw = 0f;


    public bool useIt = false;

    Camera cam;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        cam = GetComponentInChildren<Camera>();

    }

    void FixedUpdate()
    {
        if (!useIt)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }

        Vector3 move = Vector3.zero;

        if (Keyboard.current.wKey.isPressed) move += transform.forward;
        if (Keyboard.current.sKey.isPressed) move -= transform.forward;

        if (Keyboard.current.aKey.isPressed) move -= transform.right;
        if (Keyboard.current.dKey.isPressed) move += transform.right;

        if (Keyboard.current.spaceKey.isPressed) move += Vector3.up;
        if (Keyboard.current.leftCtrlKey.isPressed) move += Vector3.down;

        rb.linearVelocity = move.normalized * moveSpeed;

        // Mouse
        Vector2 mouse = Mouse.current.delta.ReadValue();

        yaw += mouse.x * mouseSensitivity;
        pitch -= mouse.y * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, -80f, 80f);

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);

        rb.MoveRotation(Quaternion.RotateTowards(
            rb.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime));
    }
}

