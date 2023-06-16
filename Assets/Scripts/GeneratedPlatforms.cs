using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject platformsPrefab;
    [SerializeField] private float speed = 0.0f;
    [SerializeField] private float moveRange = 3.0f;
    private int PLATFORM_NUM = 10;
    private float radius = 10f;
    private GameObject[] platforms;
    private Vector3[] positions;
    private float angle;
    public float rotationSpeed = 20f;
    private bool movementEnabled = true;

    private void Start()
    {
        platforms = GameObject.FindGameObjectsWithTag("TriggeredPlatform");
    }

    private void Update()
    {
        if (!movementEnabled) return;
        angle += rotationSpeed * Time.deltaTime;

        for (var i = 0; i < platforms.Length; i++)
        {
            var angleIncrement = 360f / PLATFORM_NUM;
            var currentAngle = angle + i * angleIncrement;

            var radianAngle = currentAngle * Mathf.Deg2Rad;

            var x = radius * Mathf.Cos(radianAngle);
            var y = radius * Mathf.Sin(radianAngle);
            var targetPosition = new Vector3(x + transform.position.x, y + transform.position.y, 0);

            platforms[i].transform.position = Vector3.MoveTowards(platforms[i].transform.position, targetPosition,
                rotationSpeed * Time.deltaTime);
        }
    }

    private void Awake()
    {
        platforms = new GameObject[PLATFORM_NUM];
        positions = new Vector3[PLATFORM_NUM];

        var angleIncrement = 360f / PLATFORM_NUM;
        for (var i = 0; i < platforms.Length; i++)
        {
            var currentAngle = angle + i * angleIncrement;

            var radianAngle = currentAngle * Mathf.Deg2Rad;

            var x = radius * Mathf.Cos(radianAngle);
            var y = radius * Mathf.Sin(radianAngle);
            positions[i] = new Vector3(x + transform.position.x, y + transform.position.y, 0);
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