using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject platformsPrefab;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float moveRange = 3.0f;
    private int PLATFORM_NUM = 20;
    private float radius = 5f;
    private GameObject[] platforms;
    private Vector3[] positions;
    private float angle;
    public float rotationSpeed = 2f;
    private bool movementEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        // Przechwyæ i zainicjalizuj platformy
        platforms = GameObject.FindGameObjectsWithTag("TriggeredPlatform");
    }

    // Update is called once per frame
    void Update()
    {
        if (movementEnabled)
        {
            // Zmiana k¹ta dla rotacji
            angle += rotationSpeed * Time.deltaTime;

            for (int i = 0; i < platforms.Length; i++)
            {
                // Oblicz k¹t dla i-tej platformy
                float angleIncrement = 360f / PLATFORM_NUM;
                float currentAngle = angle + (i * angleIncrement);

                // Konwertuj k¹t na radiany
                float radianAngle = currentAngle * Mathf.Deg2Rad;

                // Oblicz pozycjê platformy na podstawie równania parametrycznego okrêgu
                float x = radius * Mathf.Cos(radianAngle);
                float y = radius * Mathf.Sin(radianAngle);
                Vector3 targetPosition = new Vector3(x + transform.position.x, y + transform.position.y, 0);

                // Porusz platform¹ w kierunku punktu docelowego
                platforms[i].transform.position = Vector3.MoveTowards(platforms[i].transform.position, targetPosition, rotationSpeed * Time.deltaTime);

            }
        }
    }
    private void Awake()
    {
        platforms = new GameObject[PLATFORM_NUM];
        positions = new Vector3[PLATFORM_NUM];

        for(int i = 0; i < platforms.Length; i++)
        {
            // Oblicz k¹t dla i-tej platformy
            float angleIncrement = 360f / PLATFORM_NUM;
            float currentAngle = angle + (i * angleIncrement);

            // Konwertuj k¹t na radiany
            float radianAngle = currentAngle * Mathf.Deg2Rad;

            // Oblicz pozycjê platformy na podstawie równania parametrycznego okrêgu
            float x = radius * Mathf.Cos(radianAngle);
            float y = radius * Mathf.Sin(radianAngle);
            positions[i] = new Vector3(x+transform.position.x, y+transform.position.y, 0);
            platforms[i] = Instantiate(platformsPrefab, positions[i], Quaternion.identity);
            platforms[i].transform.parent = transform;

        }

    }
    public void EnableMovement()
    {
        movementEnabled = true;
    }

    public void DisableMovement()
    {
        movementEnabled = false;
    }
}
