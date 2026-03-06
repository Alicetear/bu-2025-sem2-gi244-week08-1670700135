using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float gravityMultiplier = 1f;
    public bool gameOver = false;

    public Animator animator;

    public ParticleSystem fxDirt;   

    public GameObject fxExplosionPrefab;

    public AudioClip audioJump;

    public AudioClip audioCrash;

    private Rigidbody rb;

    private InputAction jumpAction;
    // 5.8 add audio source variable to play crash sound

    private AudioSource audioSource;

    private bool isOnGround = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpAction = InputSystem.actions.FindAction("Jump");

        // 5.8 get audio source component, if not exist, add one
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity *= gravityMultiplier;
        animator.SetFloat("Speed_f", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (jumpAction.triggered && isOnGround)
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            isOnGround = false;
            animator.SetTrigger("Jump_trig");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            fxDirt.Play();
            audioSource.PlayOneShot(audioJump);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            animator.SetBool("Death_b", true);
            animator.SetInteger("DeatgType_int", 1);
            fxDirt.Stop();
            Instantiate(fxExplosionPrefab, transform.position, Quaternion.identity);
            audioSource.PlayOneShot(audioCrash);

        }
    }
}
