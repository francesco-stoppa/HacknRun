using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float arriveDistance = 0.1f;
    [SerializeField] private bool loop = true;

    private int currentWaypoint = 0;

    public bool stop = false;

    private void Update()
    {
        if (stop) return;

        if (waypoints.Length == 0)
            return;

        Transform target = waypoints[currentWaypoint];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime);

        transform.LookAt(target);

        if (Vector3.Distance(transform.position, target.position) <= arriveDistance)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                if (loop)
                    currentWaypoint = 0;
                else
                    enabled = false;
            }
        }

        Debug.Log(1);
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2)
            return;

        Gizmos.color = Color.green;

        for (int i = 0; i < waypoints.Length - 1; i++)
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);

        if (loop)
            Gizmos.DrawLine(waypoints[^1].position, waypoints[0].position);
    }
}

