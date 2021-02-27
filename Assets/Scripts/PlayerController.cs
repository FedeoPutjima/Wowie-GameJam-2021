using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("MOVING")]
    public float speed;
    private float moveInput;
    private Rigidbody2D rb;
    private bool facingRight = true;

    [Header("JUMPING")]
    public float jumpForce;
    public int numberOfJumps;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    [Header("DEAD BODY STUFF")]
    public Transform levelStart;
    public GameObject deadBody;

    private void Start()
    {
        transform.position = levelStart.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (facingRight == false && moveInput > 0)
        {
            FlipThePlayer();
        }
        else if (facingRight == true && moveInput < 0)
        {
            FlipThePlayer();
        }
    }

    private void Update()
    {
        if (isGrounded == true)
        {
            numberOfJumps = 1;
        }
        if (Input.GetKey(KeyCode.Space) && numberOfJumps > 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
            numberOfJumps--;
        }
    }

    void FlipThePlayer()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void ResetPosition()
    {
        LeaveDeadBody();
        transform.position = levelStart.position;
    }

    void LeaveDeadBody()
    {
        Instantiate(deadBody, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))
        {
            ResetPosition();
        }
    }
}
