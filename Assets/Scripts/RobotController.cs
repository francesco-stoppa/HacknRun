using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class RobotController : MonoBehaviour
{
    [Tooltip("Place the camera here.")]
    public Transform cam;
    private AudioListener a;
    public bool videoCamera = false;
    [SerializeField] private bool activeCharacter = false;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private float verticalVelocity;

    private Camera c;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        c = cam.gameObject.GetComponent<Camera>();

        if (c == null) return;
        c.enabled = false;
        if (cam == null) return;
        a = cam.gameObject.GetComponent<AudioListener>();
        a.enabled = false;

        if (activeCharacter)
            ActiveCharacter();
    }

    public void ActiveCharacter()
    {
        c.enabled = true;
        activeCharacter = true;
        if (a == null) return;
        a.enabled = true;
    }

    private void Update()
    {
        if (activeCharacter == false || videoCamera == true) return;

        // Input WASD
        Vector2 input = Keyboard.current == null
            ? Vector2.zero
            : new Vector2(
                (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0),
                (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0)
            );

        input = Vector2.ClampMagnitude(input, 1f);

        // Direzioni della camera sul piano XZ
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * input.y + right * input.x) * moveSpeed;

        // Gravità
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }
}
