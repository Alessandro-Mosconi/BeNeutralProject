using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    
    [SerializeField]
    int yPositivity = 1;

    private float dirX = 0f;

    private SpriteRenderer spriteRenderer;
    public Vector2 movementDirection { get; private set; }
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
            // Calcola la direzione normalizzata
            if (Input.GetAxis("HorizontalPlayer1") != 0)
            {
                movementDirection =  new Vector2(Input.GetAxis("HorizontalPlayer1"), Input.GetAxis("JumpPlayer1")).normalized;
            }

            dirX = Input.GetAxis("HorizontalPlayer1");
            isJumping = Input.GetButtonDown("JumpPlayer1");
        } else 
        {
            // Calcola la direzione normalizzata
            if (Input.GetAxis("HorizontalPlayer2") != 0)
            {
                movementDirection = new Vector2(Input.GetAxis("HorizontalPlayer2"), Input.GetAxis("JumpPlayer2"))
                    .normalized;
            }

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
