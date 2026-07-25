using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class TargetLock : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float lockTime = 2f;
    [SerializeField] private float maxDistance = 100f;

    [SerializeField] private float aimRadius = 0.5f;

    private float timer;
    private bool locked;

    [SerializeField] private float moveDuration = 1f;

    GameObject bob;

    void Awake()
    {
        if (cam == null)
            cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (cam == null) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.SphereCast(ray, aimRadius, out RaycastHit hit, maxDistance))
        {
            if (Mouse.current.leftButton.isPressed)
            {
                if (!locked)
                {
                    timer += Time.deltaTime;

                    if (timer >= lockTime)
                    {
                        locked = true;
                        Debug.Log("Target agganciato: " + hit.collider.name);

                        if(hit.collider.tag == "Player")
                        {
                            bob = hit.collider.gameObject;
                            MoveCameraTo(hit.collider.transform);
                        }
                    }
                }
            }
            else
            {
                timer = 0f;
                locked = false;
            }
        }
        else
        {
            timer = 0f;
            locked = false;
        }
    }

    public void MoveCameraTo(Transform target)
    {
        GameObject parent = transform.parent.gameObject;

        transform.SetParent(null, true);

        if (parent != null)
            Destroy(parent.gameObject);

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
        if (bob == null) return;

        RobotController rc = bob.GetComponent<RobotController>();

        if (rc == null) return;

        rc.cam.gameObject.SetActive(true);
        rc.ActiveCharacter();

        Destroy(gameObject);
    }
}

