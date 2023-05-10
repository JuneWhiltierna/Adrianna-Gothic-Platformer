using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour

{
    [Header("Movement parameters")] [Range(0.01f, 50.0f)] [SerializeField]
    private float moveSpeed = 5f; //moving speed of the player

    [SerializeField] private float jumpForce = 6.0f;

    [Space(10)] [SerializeField] private Rigidbody2D rigidbody;

    [SerializeField] private Animator animator;

    public LayerMask groundLayer;

    float rayLength = 2f;

    [SerializeField] private bool isWalking = false;

    [SerializeField] private bool isFacingRight = true;

    [SerializeField] private int score = 0;

    [SerializeField] private int lives = 3;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private int keysFound = 0;
    [SerializeField] private int keysNumber = 3;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        startPosition = transform.position;
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    void Jump()
    {
        if (IsGrounded())
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        Debug.Log("jumping");
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
        if (other.CompareTag("Bonus"))
        {
            score++;
            Debug.Log("Score: " + score);
            other.gameObject.SetActive(false);
        }

        // Sprawdzenie, czy kolizja nast�pi�a z obiektem przeciwnika
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Sprawdzenie po�o�enia gracza w chwili kolizji
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                // Zwi�kszenie liczby zdobytych punkt�w i wypisanie komunikatu o �mierci przeciwnika
                score += 10;
                Debug.Log("Killed an enemy");
            }
            else
            {
                // Zmniejszenie liczby �y� i sprawdzenie, czy gra si� sko�czy�a
                lives--;
                if (lives <= 0)
                {
                    // Wypisanie komunikatu o ko�cu gry
                    Debug.Log("Game over");
                }
                else
                {
                    // Wypisanie liczby �y� i zresetowanie pozycji gracza
                    Debug.Log("Lives left: " + lives);
                    transform.position = new Vector3(0, 0, 0);
                }
            }
        }

        if (other.gameObject.CompareTag("Key"))
        {
            Debug.Log("Keys: " + keysFound);
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Heart"))
        {
            lives++;
            Debug.Log("Lives: " + lives);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("FallLevel"))
        {
        }

        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        isWalking = false;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);

            isWalking = true;

            if (isFacingRight == false)
            {
                Debug.Log("flipL");

                Flip();
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;

            if (isFacingRight)
            {
                Debug.Log("flipR");
                Flip();
            }
        }

        ;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("jump");
            Jump();
        }

        //Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false);

        animator.SetBool("isGrounded", IsGrounded());

        animator.SetBool("isWalking", isWalking);
    }
}