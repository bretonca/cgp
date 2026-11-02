using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dana : MonoBehaviour
{
    [Header("Retro Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 11f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public Vector2 checkSize = new Vector2(0.5f, 0.2f); // Slightly taller Y to ensure it feels spawned blocks
    public LayerMask groundLayer;      // MUST hold BOTH 'Ground' and 'MagicBlock' checked

    [Header("Solomon's Wand Magic")]
    public GameObject blockPrefab;
    public LayerMask magicBlockLayer;   // MUST hold ONLY 'MagicBlock' checked

    [Tooltip("Drag your scene's Grid or Tilemap GameObject here!")]
    public Grid levelGrid;

    [Tooltip("0 = Straight out to the sides. -1 = Diagonally downward.")]
    public int verticalGridOffset = 0;

    private Rigidbody2D rb;
    private Animator anim;

    private float horizontalInput;
    private bool isFacingRight = true;
    private bool jumpRequested = false;
    private int facingDirectionInt = 1;

    public enum BlockPlacementMode
    {
        Right,
        Left,
        DiagonalDown
    }

    [Header("Block Placement")]
    public BlockPlacementMode placementMode = BlockPlacementMode.DiagonalDown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        if (levelGrid == null)
        {
            levelGrid = FindFirstObjectByType<Grid>();
        }
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Lock mechanical direction tracking from keys
        if (horizontalInput > 0f) facingDirectionInt = 1;
        else if (horizontalInput < 0f) facingDirectionInt = -1;

        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpRequested = true;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (anim != null)
            {
                anim.SetTrigger("UseWand");
            }

            CastWandMagic();
        }

        bool isCrouching =
    Input.GetKey(KeyCode.DownArrow) ||
    Input.GetKey(KeyCode.S);

        if (anim != null)
        {
            anim.SetBool("IsCrouching", isCrouching);
        }


        FlipSprite();
    }

    void FixedUpdate()
    {
        bool isCrouching =
            Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.S);

        // Stop horizontal movement while crouching
        if (isCrouching)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }

        // Jump
        if (jumpRequested)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    private bool IsGrounded()
    {
        if (groundCheck == null) return false;
        // Scans an overlapping boundary box at her feet looking for Ground or MagicBlock layers
        Collider2D hit = Physics2D.OverlapBox(groundCheck.position, checkSize, 0f, groundLayer);
        return hit != null;
    }

    private Vector3 GetGridTargetPosition()
    {
        Vector3Int currentCell = levelGrid.WorldToCell(transform.position);

        int xOffset = facingDirectionInt;
        int yOffset = 0;

        // Hold Down + E for diagonal placement
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            yOffset = -1;
        }

        Vector3Int targetCell = new Vector3Int(
            currentCell.x + xOffset,
            currentCell.y + yOffset,
            0
        );

        return levelGrid.GetCellCenterWorld(targetCell);
    }

    void CastWandMagic()
    {
        Vector3 targetPosition = GetGridTargetPosition();

        RaycastHit2D hitMagic = Physics2D.Raycast(targetPosition, Vector2.zero, 0f, magicBlockLayer);
        RaycastHit2D hitSolid = Physics2D.Raycast(targetPosition, Vector2.zero, 0f, groundLayer);

        if (hitMagic.collider != null)
        {
            MagicBlock magicBlockComponent = hitMagic.collider.GetComponent<MagicBlock>();
            if (magicBlockComponent != null)
            {
                magicBlockComponent.TriggerBreak();
            }
            else
            {
                Destroy(hitMagic.collider.gameObject);
            }
        }
        else if (hitSolid.collider == null)
        {
            if (blockPrefab)
            {
                Instantiate(blockPrefab, targetPosition, Quaternion.identity);
            }
        }
    }



    void FlipSprite()
    {
        // FIXED VISUAL FLIP: Swapped comparison logic to match your native sprite orientation
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(groundCheck.position, checkSize);
        }

        Gizmos.color = Color.green;
        if (levelGrid != null)
        {
            Gizmos.DrawWireCube(GetGridTargetPosition(), levelGrid.cellSize);
        }
        else
        {
            Gizmos.DrawWireCube(GetGridTargetPosition(), Vector3.one);
        }
    }
}
