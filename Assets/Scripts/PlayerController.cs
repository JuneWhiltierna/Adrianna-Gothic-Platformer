using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour

{
    [Header("Movement parameters")] [Range(0.01f, 50.0f)] [SerializeField]
    private float moveSpeed = 5f;
    private float rayLength = 2f;
    private AudioSource source;
    private bool playStep = true;


    [SerializeField] private float jumpForce = 6.0f;
    [Space(10)] [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isWalking = false;
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private int lives = 3;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private int keysFound = 0;
    [SerializeField] private int keysNumber = 3;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip enemySound;
    [SerializeField] private AudioClip keySound;
    [SerializeField] private AudioClip healthLostSound;
    [SerializeField] private AudioClip healthGainedSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private List<AudioClip> stepSounds;

    private Vector3 pausePosition;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        source = GetComponent<AudioSource>();
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    private void Update()
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
    
    private IEnumerator WaitForStep()
    {
        var rnd = Random.Range(0, stepSounds.Count);
        var stepSound = stepSounds[rnd];
        source.PlayOneShot(stepSound,AudioListener.volume);
        yield return new WaitForSeconds(0.333f);
        playStep = true;
    }

    private void UpdateInternal()
    {
        isWalking = false;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);

            isWalking = true;

            if (isFacingRight == false) Flip();
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;

            if (isFacingRight) Flip();
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) Jump();

        if ( IsGrounded() && isWalking && playStep)
        {
            playStep = false;
            StartCoroutine(WaitForStep());
        }
        
        animator.SetBool("isGrounded", IsGrounded());
        animator.SetBool("isWalking", isWalking);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            source.PlayOneShot(jumpSound,AudioListener.volume);
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
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
        if (other.CompareTag("Bonus"))
        {
            GameManager.Instance.AddPoints(10);
            other.gameObject.SetActive(false);
            source.PlayOneShot(coinSound,AudioListener.volume);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                GameManager.Instance.AddPoints(20);
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                source.PlayOneShot(enemySound,AudioListener.volume);

            }
            else
            {
                lives--;
                source.PlayOneShot(healthLostSound,AudioListener.volume);
                GameManager.Instance.RemoveHeart();
                if (lives <= 0)
                {
                }
                else
                {
                    transform.position = new Vector3(0, 0, 0);
                }
            }
        }

        if (other.gameObject.CompareTag("Key"))
        {
            source.PlayOneShot(keySound,AudioListener.volume);
            GameManager.Instance.AddPoints(100);
            keysFound++;
            GameManager.Instance.AddKeys(1);
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Heart"))
        {
            source.PlayOneShot(healthGainedSound,AudioListener.volume);
            lives++;
            if (lives >= 4)
                lives = 3;
            else
                GameManager.Instance.AddHeart();
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Finish"))
            if (keysFound == 3)
            {
                GameManager.Instance.AddPoints(lives * 100);
                GameManager.Instance.LevelCompleted();
            }

        if (other.CompareTag("MovingPlatform")) transform.SetParent(other.transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform")) transform.SetParent(null);
    }
}