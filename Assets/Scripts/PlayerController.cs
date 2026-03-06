using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float gravityMultiplier = 1f;
    public bool gameOver = false;
    // 4.1 add animator variable
    public Animator animator;
    // 5.2 add particle system variable for dirt splatter effect
    public ParticleSystem fxDirtSplatter;
    // 5.3 add particle system variable for explosion smoke effect
    public ParticleSystem fxExplosionSmoke;
    // 5.7 add audio clip variable for crash sound
    public AudioClip crashSound;
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

        // 4.1 set animator parameter's `Speed_f` to 1f at start to make player play running animation
        if (animator != null)
        {
            animator.SetFloat("Speed_f", 1f);
        }
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
            // 4.2 set animator trigger `Jump_trig` to make player play jump animation
            animator.SetTrigger("Jump_trig");
            // 5.2 stop dirt splatter effect when player jumps
            fxDirtSplatter.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            // 5.2 play dirt splatter effect when player lands on the ground
            fxDirtSplatter.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            // 4.4 set animator parameter `Death_b` to true to make player play death animation
            animator.SetBool("Death_b", true);
            animator.SetInteger("DeathType_int", 1);
            // 5.3 instantiate explosion smoke effect when player hits the obstacle
            Instantiate(fxExplosionSmoke, collision.contacts[0].point, Quaternion.identity);
            // 5.4 stop dirt splatter effect when player dies
            fxDirtSplatter.Stop();
            // 5.8 play crash sound effect when player hits the obstacle
            audioSource.PlayOneShot(crashSound);
        }
    }
}
