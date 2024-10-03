using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class CharacterAnimator : MonoBehaviour
{
    // Reference to the Animator component
    private Animator animator;
    // Reference to the Rigidbody component for movement
    private Rigidbody2D rb;

    // Movement variables
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to this object
        animator = GetComponent<Animator>();
        // Get the Rigidbody2D component for physics-based movement
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle movement and animations
        HandleMovement();
    }

    void HandleMovement()
    {
        // Get horizontal input (-1 for left, 1 for right, 0 for idle)
        float move = Input.GetAxis("Horizontal");

        // Set animator parameter to control walking animation
        if (move != 0)
        {
            // Character is walking
            animator.SetBool("isWalking", true);

            // Move the character horizontally
            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

            // Flip the character sprite depending on movement direction
            if (move > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Face right
            }
            else if (move < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Face left
            }
        }
        else
        {
            // Character is idle
            animator.SetBool("isWalking", false);

            // Set velocity to zero for idle
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }
}

