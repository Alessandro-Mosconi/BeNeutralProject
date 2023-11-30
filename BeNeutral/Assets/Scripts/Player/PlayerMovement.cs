using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    
    [SerializeField]
    int yPositivity = 1;

    private float dirX = 0f;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = rb.gravityScale * yPositivity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool isJumping = false;
        
        if (yPositivity > 0)
        {
            dirX = Input.GetAxis("HorizontalPlayer1");
            isJumping = Input.GetButtonDown("JumpPlayer1");
        } else 
        {
            dirX = Input.GetAxis("HorizontalPlayer2");
            isJumping = Input.GetButtonDown("JumpPlayer2");
        }
        rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);

        if (isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, yPositivity * 7f);
        }
        
        UpdateAnimationUpdate();
    }

    private void UpdateAnimationUpdate()
    {
        string varRunning = "running";
        if (yPositivity > 0)
        {
            varRunning = "running";
        }
        else
        {
            varRunning = "running_2";
        }
        if (dirX > 0)
        {
            spriteRenderer.flipX = false;
            animator.SetBool(varRunning, true);
        } else if (dirX < 0)
        {
            spriteRenderer.flipX = true;
            animator.SetBool(varRunning, true);
        }
        else
        {
            animator.SetBool(varRunning, false);
        }
    }
}
