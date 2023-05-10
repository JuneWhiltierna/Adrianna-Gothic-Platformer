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

        // Pobierz po³o¿enie bie¿¹cego waypointa
        Vector2 targetPosition = waypoints[currentWaypoint].transform.position;

        // Oblicz odleg³oœæ miêdzy bie¿¹cym po³o¿eniem platformy a punktem docelowym
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);

        // Przesuñ platformê w stronê punktu docelowego
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // SprawdŸ, czy platforma dotar³a do punktu docelowego
        if (distanceToTarget < 0.1f)
        {
            // PrzejdŸ do nastêpnego waypointa
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }

    }
}
