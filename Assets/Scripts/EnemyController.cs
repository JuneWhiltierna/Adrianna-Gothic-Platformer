using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Range(0.01f, 50.0f)] [SerializeField] private float moveSpeed = 5f; //moving speed of the enemy
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private Animator animator;

    [SerializeField] private float startPositionX;
    [SerializeField] private float moveRange = 3.0f;

    [SerializeField] private bool movingRight;

    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            if (transform.position.y < other.gameObject.transform.position.y)
            {
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
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