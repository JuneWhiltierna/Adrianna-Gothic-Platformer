using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

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

    private Vector3 pausePosition;

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
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentGameState == GameState.GAME)
        {
            UpdateInternal();
            pausePosition = transform.position;
            animator.speed = 1;

        }
        else
        {
            transform.position = pausePosition;
            animator.speed = 0;
        }
    }
    private void UpdateInternal()
    {
        isWalking = false;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);

            isWalking = true;

            if (isFacingRight == false)
            {
                Flip();
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;

            if (isFacingRight)
            {
                Flip();
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }

        //Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false);

        animator.SetBool("isGrounded", IsGrounded());

        animator.SetBool("isWalking", isWalking);
    }
    void Jump()
    {
        if (IsGrounded())
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
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

            GameManager.Instance.AddPoints(1);
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
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
            keysFound++;
            GameManager.Instance.AddKeys(1);
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Heart"))
        {
            lives++;
            GameManager.Instance.AddHeart();
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

 
}