using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dana : MonoBehaviour
{
    [Header("Retro Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 11f;

    [Header("Ground Detection")]
    public Transform groundCheck;      // Drop your GroundCheck empty object here!
    public Vector2 checkSize = new Vector2(0.5f, 0.1f); // Dimensions of the detection box
    public LayerMask groundLayer;      // Set this to your 'Ground' layer

    private Rigidbody2D rb;
    private Animator anim;

    private float horizontalInput;
    private bool isFacingRight = true;
    private bool jumpRequested = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
        }

        // Capture jump button press cleanly
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpRequested = true;
        }

        FlipSprite();
    }

    void FixedUpdate()
    {
        // Crisp, instant retro horizontal movement
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Execute jump safely inside the physics loop
        if (jumpRequested)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    private bool IsGrounded()
    {
        if (groundCheck == null) return false;

        // Creates an invisible sensory block at her feet that ONLY interacts with the floor layer
        Collider2D hit = Physics2D.OverlapBox(groundCheck.position, checkSize, 0f, groundLayer);
        return hit != null;
    }

    void FlipSprite()
    {
        if (isFacingRight && horizontalInput > 0f || !isFacingRight && horizontalInput < 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // Draws a blue wireframe box in your Scene View editor so you can easily verify its position!
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(groundCheck.position, checkSize);
        }
    }
}

