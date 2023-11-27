using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField]
    int yPositivity = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = rb.gravityScale * yPositivity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float dirX;
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
    }
}
