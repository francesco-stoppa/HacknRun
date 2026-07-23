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

    void Awake()
    {
        if (cam == null)
            cam = GetComponent<Camera>();
    }

    void Update()
    {
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

                        if(hit.collider.name == "Cube")
                        {
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
        Debug.Log(parent.gameObject.name);
        transform.SetParent(null, true);

        if (parent != null)
            Destroy(parent.gameObject);

        StopAllCoroutines();
        StartCoroutine(MoveRoutine(target.position));
    }

    IEnumerator MoveRoutine(Vector3 targetPosition)
    {

        // transform.SetParent(null, true);

        Vector3 startPosition = transform.position;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / moveDuration;

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        transform.position = targetPosition;
    }

}

