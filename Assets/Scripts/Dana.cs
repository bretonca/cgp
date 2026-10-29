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

    [Header("Solomon's Wand Magic")]
    public GameObject blockPrefab;      // Drop your MagicBlock prefab here!
    public Transform wandTargetRight;   // Empty object for target on the right
    public Transform wandTargetLeft;    // Empty object for target on the left
    public float checkBlockRadius = 0.4f;

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

        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.E))
        {
            CastWandMagic();
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

    void CastWandMagic()
    {
        Transform activeTarget = isFacingRight ? wandTargetRight : wandTargetLeft;
        if (activeTarget == null) return;

        Collider2D existingBlock = Physics2D.OverlapCircle(activeTarget.position, checkBlockRadius, groundLayer);

        if (existingBlock != null)
        {
            MagicBlock magicBlockComponent = existingBlock.GetComponent<MagicBlock>();

            if (magicBlockComponent != null)
            {
                magicBlockComponent.TriggerBreak();
            }
            else
            {
                Destroy(existingBlock.gameObject);
            }

            // Commented out to stop parameters error:
            // if (anim != null) anim.SetTrigger("UseWand");
        }
        else
        {
            Instantiate(blockPrefab, activeTarget.position, Quaternion.identity);

            // Commented out to stop parameters error:
            // if (anim != null) anim.SetTrigger("UseWand");
        }
    } // <-- THIS WAS MISSING! This closes the CastWandMagic function properly.

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

    // Draws wireframe helpers in your Scene View editor so you can easily verify positions!
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(groundCheck.position, checkSize);
        }

        if (wandTargetRight != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(wandTargetRight.position, checkBlockRadius);
        }

        if (wandTargetLeft != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wandTargetLeft.position, checkBlockRadius);
        }
    }
}
