using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [Range(0.01f, 50.0f)] [SerializeField] private float moveSpeed = 5f; //moving speed of the enemy
    [SerializeField] private float startPositionX;
    [SerializeField] private float moveRange = 3.0f;
    [SerializeField] private bool isFacingRight = true;


    private void Awake()
    {
        startPositionX = transform.position.x;
    }

    private void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    private void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
    }

    private void Update()
    {
        {
            if (isFacingRight)
            {
                if (transform.position.x < startPositionX + moveRange)
                {
                    MoveRight();
                }
                else
                {
                    Flip();
                    MoveLeft();
                }
            }
            else
            {
                if (transform.position.x > startPositionX - moveRange)
                {
                    MoveLeft();
                }
                else
                {
                    Flip();
                    MoveRight();
                }
            }
        }
    }
}