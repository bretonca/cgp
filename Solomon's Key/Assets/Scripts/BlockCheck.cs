using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class BlockCheck : MonoBehaviour
{
    private Collider2D detectedCollider;

    public bool BlockHovered => detectedCollider != null;

    public bool TryBreakBlock()
    {
        if (detectedCollider == null ||
            !detectedCollider.CompareTag("BreakableBlock"))
        {
            return false;
        }

        Destroy(detectedCollider.gameObject);
        detectedCollider = null;

        return true;
    }

    public bool TryBreakBlock(Vector2 position, Vector2 checkSize)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            position,
            checkSize,
            0f
        );

        foreach (Collider2D collider in colliders)
        {
            if (!collider.CompareTag("BreakableBlock"))
            {
                continue;
            }

            Destroy(collider.gameObject);
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BreakableBlock"))
        {
            detectedCollider = other;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == detectedCollider)
        {
            detectedCollider = null;
        }
    }
}
