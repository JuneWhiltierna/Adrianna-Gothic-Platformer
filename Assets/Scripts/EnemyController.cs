using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyController : MonoBehaviour
{
    [Range(0.01f, 50.0f)] [SerializeField] private float moveSpeed = 5f; //moving speed of the enemy
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private Animator animator;

    [SerializeField] private float startPositionX;
    [SerializeField] private float moveRange = 3.0f;

    [SerializeField] private bool movingRight;
    //[SerializeField] private bool movingLeft;

    private void Awake()
    {
        //rigidbody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        startPositionX = this.transform.position.x;
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
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdzenie, czy kolizja nast�pi�a z obiektem przeciwnika
        if (other.gameObject.CompareTag("Player"))
        {
            // Sprawdzenie po�o�enia gracza w chwili kolizji
            if (transform.position.y < other.gameObject.transform.position.y)
            {
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
        }
    }

    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
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
        //private void Flip()
        //{
        //    isFacingRight = !isFacingRight;
        //    Vector3 theScale = transform.localScale;
        //    theScale.x *= -1;
        //    transform.localScale = theScale;
        //}
    }
}