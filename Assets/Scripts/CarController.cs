using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public float acceleration = 20f;
    public float turnSpeed = 100f;
    public float maxSpeed = 15f;

    Rigidbody rb;

    public bool useIt = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!useIt)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }


        float vertical = 0;

        if (Keyboard.current.wKey.isPressed) vertical = 1;
        if (Keyboard.current.sKey.isPressed) vertical = -1;

        // Mouse
        float mouseX = Mouse.current.position.ReadValue().x;
        float steering = ((mouseX / Screen.width) - 0.5f) * 2f;
        steering = Mathf.Clamp(steering, -1f, 1f);

        // Accelerazione
        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * vertical * acceleration, ForceMode.Acceleration);
        }

        // Sterzo
        if (rb.linearVelocity.magnitude > 0.5f)
        {
            rb.MoveRotation(
                rb.rotation *
                Quaternion.Euler(0, steering * turnSpeed * Time.fixedDeltaTime, 0)
            );
        }
    }
}

