using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    
    [SerializeField] public int yPositivity = 1;
    [SerializeField] public int playerNumber = 1;

    private float dirX = 0f;

    private SpriteRenderer spriteRenderer;
    public Vector2 movementDirection { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool isJumping = false;

        if (playerNumber == 1)
        {
            if (Input.GetAxis("HorizontalPlayer1") != 0)
            {
                movementDirection =  new Vector2(Input.GetAxis("HorizontalPlayer1"), Input.GetAxis("JumpPlayer1")).normalized;
            }

            dirX = Input.GetAxis("HorizontalPlayer1");
            isJumping = Input.GetButtonDown("JumpPlayer1");
        } else if (playerNumber == 2)
        {
            if (Input.GetAxis("HorizontalPlayer2") != 0)
            {
                movementDirection = new Vector2(Input.GetAxis("HorizontalPlayer2"), Input.GetAxis("JumpPlayer2"))
                    .normalized;
            }

            dirX = Input.GetAxis("HorizontalPlayer2");
            isJumping = Input.GetButtonDown("JumpPlayer2");
        }
        
        
        if (yPositivity > 0)
        {
            rb.gravityScale = Math.Abs(rb.gravityScale);
            
            Quaternion rotation = transform.rotation;
            rotation.x = 0;
            transform.rotation = rotation;
        } else 
        {
            rb.gravityScale = Math.Abs(rb.gravityScale)*-1;
            Quaternion rotation = transform.rotation;
            rotation.x = -180;
            transform.rotation = rotation;
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

        if (playerNumber == 1)
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
