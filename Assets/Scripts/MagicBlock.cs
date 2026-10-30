using UnityEngine;

public class MagicBlock : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    private bool isBreaking = false;

    [Header("Speed Settings")]
    [Tooltip("How many seconds to wait before wiping the block from the scene after pressing E")]
    public float destructionDelay = 0.2f; // Lower this number to make it even faster!

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    public void TriggerBreak()
    {
        if (isBreaking) return;
        isBreaking = true;

        // Turn off the collider instantly so Dana can instantly move through the space
        if (coll != null) coll.enabled = false;

        // Play the crumble animation state
        if (anim != null) anim.SetTrigger("Break");

        // Destroy the block object after your exact custom delay speed
        Destroy(gameObject, destructionDelay);
    }
}
