using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private Transform _originalParent;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    [SerializeField] public int gravityDirection = 1;
    [SerializeField] public int playerNumber = 1;

    private bool isGrounded;
    private float dirX = 0f;

    public Vector2 movementDirection { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        _originalParent = transform.parent;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleGravity();

        dirX = Input.GetAxis("HorizontalPlayer" + playerNumber);

        if (dirX != 0)
        {
            movementDirection =  new Vector2(Input.GetAxis("HorizontalPlayer" + playerNumber), Input.GetAxis("JumpPlayer" + playerNumber)).normalized;
        }
        
        rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);
        
        if(Input.GetButton("JumpPlayer" + playerNumber)  && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, gravityDirection * 8f);
            isGrounded = false;
        }

        UpdateAnimationUpdate();
    }

    private void HandleGravity()
    {
        if (gravityDirection > 0)
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
    
    public void setParent(Transform newParent)
    {
        _originalParent = transform.parent;
        transform.parent = newParent;
    }
    
    public void resetParent()
    {
        transform.parent = _originalParent;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.contacts.Length > 0)
        {
            for (int i = 0; i < other.contacts.Length; i++)
            {
                ContactPoint2D contact = other.contacts[i];

                if (gravityDirection * Vector2.Dot(contact.normal, Vector2.up) > 0.5f)
                {
                    if (other.gameObject.layer == LayerMask.NameToLayer("Terrain") ||
                        other.gameObject.layer == LayerMask.NameToLayer("External-objects"))
                    {
                        isGrounded = true;
                        return;
                    }
                }
            }
        }
        
    }
}
