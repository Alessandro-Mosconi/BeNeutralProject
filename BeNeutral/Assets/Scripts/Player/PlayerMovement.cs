using System;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private enum MovementState
    {
        idle,
        running,
        jumping,
        falling
    };
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] public int gravityDirection = 1;
    [SerializeField] public int playerNumber = 1;
    public Vector2 movementDirection { get; private set; }

    private Transform _originalParent;
    private bool isGrounded;
    private float dirX = 0f;
    private MovementState state = MovementState.idle;
    private String animationState;
    private SpriteRenderer spriteRenderer;
    private bool inputDisabledUntilKeyup = false;
    private float watchedAxisForInputEnable;
    private Vector2 prevImpulse = Vector2.zero;
    private Vector2 _pseudoForce = Vector2.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        animationState = "state" + playerNumber;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        _originalParent = transform.parent;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleGravity();

        if (!inputDisabledUntilKeyup)
        {
            dirX = Input.GetAxis("HorizontalPlayer" + playerNumber);

            if (dirX != 0)
            {
                movementDirection =  new Vector2(Input.GetAxis("HorizontalPlayer" + playerNumber), Input.GetAxis("JumpPlayer" + playerNumber)).normalized;
            }
            
            rb.velocity = new Vector2(dirX * 7f + _pseudoForce.x, rb.velocity.y);
        
            if(Input.GetButton("JumpPlayer" + playerNumber)  && isGrounded)
            {
                AudioManager.Instance.PlayJumpPlayer();
                rb.velocity = new Vector2(rb.velocity.x, gravityDirection * 8f);
                isGrounded = false;
            }
        }
        else
        {
            CheckEnableInput();
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

        if (dirX > 0)
        {
            spriteRenderer.flipX = false;
            state = MovementState.running;
            if(isGrounded)
                AudioManager.Instance.PlayWalkingPlayer();
        } else if (dirX < 0)
        {
            spriteRenderer.flipX = true;
            state = MovementState.running;
            if(isGrounded)
                AudioManager.Instance.PlayWalkingPlayer();
        }
        else
        {
            state = MovementState.idle;
            AudioManager.Instance.StopWalkingPlayerSound();
        }

        if (!isGrounded && gravityDirection * rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        } 
        
        if (!isGrounded && gravityDirection * rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        } 
        
        animator.SetInteger(animationState,(int) state);
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

    public void StopMovementUntilKeyup()
    {
        inputDisabledUntilKeyup = true;
        watchedAxisForInputEnable = Input.GetAxis("HorizontalPlayer" + playerNumber);
        rb.velocity = new Vector2(0, rb.velocity.y);
        dirX = 0;
    }

    private void CheckEnableInput()
    {
        if (Math.Abs(Input.GetAxis("HorizontalPlayer" + playerNumber)) <= 0.001f)
        {
            inputDisabledUntilKeyup = false;
            watchedAxisForInputEnable = 0;
        }
    }

    public void SetForce(Vector2 force) //This is a pseudo-force that makes player input less intense!
    {
        _pseudoForce = force;
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
