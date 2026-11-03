using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Puncher : GroundedEnemy
{
    private bool wasTouchingWall;
    private MagicBlock magicBlock;
    Collider2D wallCollision;

    void Start()
    {

    }

    void Update()
    {
        if (facingRight)
        {
            if (rightGroundCheck.IsTouchingLayers(LayerMask.GetMask("Ground", "MagicBlock")))
            {
                rb.velocity = Vector2.right * speed;
            }
            else
            {
                facingRight = !facingRight;
                rb.velocity = Vector2.zero;
                Debug.Log("turning left!");
            }

            if (rightWallCheck.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                facingRight = !facingRight;
                rb.velocity = Vector2.zero;
            } else if (rightWallCheck.IsTouchingLayers(LayerMask.GetMask("MagicBlock")))
            {
                magicBlock = GetMagicBlockFromWallCheck(rightWallCheck);

                if (magicBlock != null)
                {
                    magicBlock.TriggerBreak();
                }

                facingRight = !facingRight;
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            if (leftGroundCheck.IsTouchingLayers(LayerMask.GetMask("Ground", "MagicBlock")))
            {
                rb.velocity = Vector2.left * speed;
            } 
            else
            {
                facingRight = !facingRight;
                rb.velocity = Vector2.zero;
                Debug.Log("turning right!");
            }

            if (leftWallCheck.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                facingRight = !facingRight;
                rb.velocity = Vector2.zero;
            } else if (leftWallCheck.IsTouchingLayers(LayerMask.GetMask("MagicBlock")))
            {
                magicBlock = GetMagicBlockFromWallCheck(leftWallCheck);

                if (magicBlock != null)
                {
                    magicBlock.TriggerBreak();
                }

                facingRight = !facingRight;
                rb.velocity = Vector2.zero;
            }
        }
    }

    private MagicBlock GetMagicBlockFromWallCheck(BoxCollider2D wallCheck)
    {
        Collider2D[] contacts = new Collider2D[10];
        int contactCount = wallCheck.GetContacts(contacts);

        int magicBlockLayer = LayerMask.NameToLayer("MagicBlock");

        for (int i = 0; i < contactCount; i++)
        {
            if (contacts[i] == null || contacts[i].gameObject.layer != magicBlockLayer)
            {
                continue;
            }

            wallCollision = contacts[i];

            MagicBlock block = wallCollision.GetComponent<MagicBlock>();

            if (block == null)
            {
                block = wallCollision.GetComponentInParent<MagicBlock>();
            }

            if (block != null)
            {
                return block;
            }
        }

        return null;
    }
}
