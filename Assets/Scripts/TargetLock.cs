using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class TargetLock : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float lockTime = 2f;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float aimRadius = 0.5f;
    [SerializeField] private float moveDuration = 1f;

    [Header("Crosshair Settings")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite validTargetSprite;

    private float timer;
    private bool locked;
    GameObject goToHack;

    void Awake()
    {
        if (cam == null)
            cam = GetComponent<Camera>();

        if (crosshairImage != null && normalSprite != null)
            crosshairImage.sprite = normalSprite;
    }

    void Update()
    {
        if (cam == null) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // 1. VERIFICA SE IL RAGGIO STA COLPENDO QUALCOSA
        if (Physics.SphereCast(ray, aimRadius, out RaycastHit hit, maxDistance))
        {
            // Controlliamo se è un target valido (es. con Tag "Player")
            bool isValidTarget = hit.collider.CompareTag("Player");

            // --- CAMBIO SPRITE (Funziona semplicemente guardando l'oggetto!) ---
            UpdateCrosshairSprite(isValidTarget);

            // --- LOGICA DI HACKING (Solo se il target è valido e si tiene premuto Click Sinistro) ---
            if (isValidTarget && Mouse.current.leftButton.isPressed)
            {
                if (!locked)
                {
                    timer += Time.deltaTime;

                    if (timer >= lockTime)
                    {
                        locked = true;
                        Debug.Log("Target agganciato: " + hit.collider.name);

                        goToHack = hit.collider.gameObject;

                        NpcMovement nm = goToHack.GetComponent<NpcMovement>();
                        if (nm != null)
                            nm.stop = true;

                        RobotController rc = goToHack.GetComponent<RobotController>();
                        if (rc != null)
                            MoveCameraTo(rc.characterCamera);
                        else
                            Debug.LogError("RobotController non trovato!");
                    }
                }
            }
            else
            {
                // Se non premiamo il tasto (o se l'oggetto puntato non è hackerabile), resettiamo solo il timer dell'hack
                timer = 0f;
                locked = false;
            }
        }
        else
        {
            // Il raggio non colpisce niente: resettiamo sia lo sprite che il timer dell'hack
            UpdateCrosshairSprite(false);
            timer = 0f;
            locked = false;
        }
    }

    // Gestione pulita dello sprite in base al fatto che stiamo puntando un target o meno
    private void UpdateCrosshairSprite(bool isTargetValid)
    {
        if (crosshairImage == null) return;

        if (isTargetValid && validTargetSprite != null)
        {
            if (crosshairImage.sprite != validTargetSprite)
                crosshairImage.sprite = validTargetSprite;
        }
        else if (normalSprite != null)
        {
            if (crosshairImage.sprite != normalSprite)
                crosshairImage.sprite = normalSprite;
        }
    }

    public void MoveCameraTo(Transform target)
    {
        if (transform.parent != null)
        {
            GameObject parentObject = transform.parent.gameObject;
            transform.SetParent(null, true);
            Destroy(parentObject);
        }

        StopAllCoroutines();
        StartCoroutine(MoveRoutine(target.position));
    }

    IEnumerator MoveRoutine(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        CompleateTransition();
    }

    void CompleateTransition()
    {
        if (goToHack == null) return;

        RobotController rc = goToHack.GetComponent<RobotController>();
        if (rc == null) return;

        rc.characterCamera.gameObject.SetActive(true);
        rc.ActiveCharacter();

        Destroy(gameObject);
    }
}