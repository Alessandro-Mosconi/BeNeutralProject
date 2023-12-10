using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    [SerializeField] public int yPositivity = 1;
    [SerializeField] public int playerNumber = 1;

    private float dirX = 0f;

    public Vector2 movementDirection { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleGravity();
        bool isJumping = false;

        dirX = Input.GetAxis("HorizontalPlayer" + playerNumber);
        isJumping = Input.GetButton("JumpPlayer" + playerNumber);
        
        if (dirX != 0)
        {
            movementDirection =  new Vector2(Input.GetAxis("HorizontalPlayer" + playerNumber), Input.GetAxis("JumpPlayer" + playerNumber)).normalized;
        }
        
        rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);

        if (isJumping && Math.Round(rb.velocity.y,3)  == 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, yPositivity * 8f);
        }

        UpdateAnimationUpdate();
    }

    private void HandleGravity()
    {
        if (yPositivity > 0)
        {
            rb.gravityScale = Math.Abs(rb.gravityScale);
            Quaternion rotation = transform.rotation;
            rotation.x = 0;
            transform.rotation = rotation;
        }
        else
        {
            rb.gravityScale = Math.Abs(rb.gravityScale) * -1;
            Quaternion rotation = transform.rotation;
            rotation.x = -180;
            transform.rotation = rotation;
        }
    }

    private void UpdateAnimationUpdate()
    {
        string varRunning = "runningPlayer" + playerNumber;
        
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
