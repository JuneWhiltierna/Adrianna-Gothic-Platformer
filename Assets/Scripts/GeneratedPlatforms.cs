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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (transform.position.y < positions[0].y + moveRange) 
        //{

        //}
    }
    private void Awake()
    {
        platforms = new GameObject[PLATFORM_NUM];
        positions = new Vector3[PLATFORM_NUM];

        for(int i = 0; i < platforms.Length; i++)
        {
            // Oblicz k¹t dla i-tej platformy
            float angle = i * (360f / PLATFORM_NUM);

            // Konwertuj k¹t na radiany
            float radianAngle = angle * Mathf.Deg2Rad;

            // Oblicz pozycjê platformy na podstawie równania parametrycznego okrêgu
            float x = radius * Mathf.Cos(radianAngle);
            float y = radius * Mathf.Sin(radianAngle);
            positions[i] = new Vector3(x+transform.position.x, y+transform.position.y, 0);
            platforms[i] = Instantiate(platformsPrefab, positions[i], Quaternion.identity);
            platforms[i].transform.parent = transform;

        }
    }
}
