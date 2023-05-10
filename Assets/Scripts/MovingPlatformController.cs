using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [Range(0.01f, 50.0f)][SerializeField] private float moveSpeed = 5f; //moving speed of the enemy
    [SerializeField] private float startPositionX;
    [SerializeField] private float moveRange = 3.0f;
    [SerializeField] private bool isFacingRight = true;


    // Start is called before the first frame update
    private void Awake()
    {
        //rigidbody = GetComponent<Rigidbody2D>();

        //animator = GetComponent<Animator>();
        startPositionX = transform.position.x;
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);

    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
    }
    // Update is called once per frame
    void Update()
    {
        {
            // Sprawdzenie kierunku ruchu przeciwnika
            if (isFacingRight)
            {
                // Poruszanie si� w prawo, je�li nie przekroczono granicy
                if (transform.position.x < startPositionX + moveRange)
                {
                    MoveRight();
                }
                else
                {
                    // Zmiana kierunku ruchu i poruszanie si� w lewo
                    Flip();
                    MoveLeft();
                }
            }
            else
            {
                // Poruszanie si� w lewo, je�li nie przekroczono granicy
                if (transform.position.x > startPositionX - moveRange)
                {
                    MoveLeft();
                }
                else
                {
                    // Zmiana kierunku ruchu i poruszanie si� w prawo
                    Flip();
                    MoveRight();
                }
            }

        }
    }
}
