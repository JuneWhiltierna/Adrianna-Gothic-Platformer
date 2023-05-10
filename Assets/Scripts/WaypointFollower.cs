using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 1.0f;
    int currentWaypoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Length == 0)
            return;

        // Pobierz po�o�enie bie��cego waypointa
        Vector2 targetPosition = waypoints[currentWaypoint].transform.position;

        // Oblicz odleg�o�� mi�dzy bie��cym po�o�eniem platformy a punktem docelowym
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);

        // Przesu� platform� w stron� punktu docelowego
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Sprawd�, czy platforma dotar�a do punktu docelowego
        if (distanceToTarget < 0.1f)
        {
            // Przejd� do nast�pnego waypointa
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }

    }
}
