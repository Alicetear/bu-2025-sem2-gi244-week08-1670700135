using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float gravityMultiplier = 1f;
    public bool gameOver = false;
    // 4.1 add animator variable
    public Animator animator;
    private Rigidbody rb;
    private InputAction jumpAction;

    private bool isOnGround = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpAction = InputSystem.actions.FindAction("Jump");
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
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            // 4.4 set animator parameter `Death_b` to true to make player play death animation
            animator.SetBool("Death_b", true);
            animator.SetInteger("DeathType_int", 1);
        }
    }
}
