using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class RobotController : MonoBehaviour
{
    [Header("Need to compilates")]
    public Transform characterCamera;
    public E_CharacterYpe characterType;
    [SerializeField] private bool activeCharacter = false;

    [Header("Stats")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;


    private CharacterController controller;
    private CharacterTimer timer;
    private float verticalVelocity;


    // player needs
    Camera c;
    AudioListener a;
    TargetLock tl;
    bool videoCamera = false;
    CameraController cc;

    private void Awake()
    {
        gameObject.layer = 0;


        timer = GetComponent<CharacterTimer>();
        controller = GetComponent<CharacterController>();
        c = characterCamera.gameObject.GetComponent<Camera>();

        tl = characterCamera.gameObject.GetComponent<TargetLock>();
        tl.enabled = false;

        cc = characterCamera.gameObject.GetComponent<CameraController>();

        cc.SetType();

        switch (characterType)
        {
            case E_CharacterYpe.roomba:
                cc.SetType(true, true);
                break;
            case E_CharacterYpe.robot:
                cc.SetType(true);
                break;
            case E_CharacterYpe.camera:
                videoCamera = true;
                break;
        }

        if (c == null) return;
        c.enabled = false;
        if (characterCamera == null) return;
        a = characterCamera.gameObject.GetComponent<AudioListener>();
        a.enabled = false;

        if (activeCharacter)
            ActiveCharacter();
    }

    public void ActiveCharacter()
    {
        switch(characterType)
        {
            case E_CharacterYpe.roomba:
                gameObject.layer = 0;
                
                break;
            case E_CharacterYpe.robot:
                gameObject.layer = 3;

                break;
            case E_CharacterYpe.camera:
                videoCamera = true;
                gameObject.layer = 6;
                break;
        }

        c.enabled = true;
        activeCharacter = true;
        tl.enabled = true;

        if(timer != null)
            timer.StartTimer();

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
        Vector3 forward = characterCamera.forward;
        Vector3 right = characterCamera.right;

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
