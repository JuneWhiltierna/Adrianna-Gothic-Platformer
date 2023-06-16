using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 1.0f;
    private int currentWaypoint = 0;

    private void Update()
    {
        if (waypoints.Length == 0)
            return;

        Vector2 targetPosition = waypoints[currentWaypoint].transform.position;

        var distanceToTarget = Vector2.Distance(transform.position, targetPosition);

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (distanceToTarget < 0.1f) currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }
}