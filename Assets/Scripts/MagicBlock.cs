using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBlock : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    private bool isBreaking = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    public void TriggerBreak()
    {
        if (isBreaking) return; // Prevent double triggers
        isBreaking = true;

        // Turn off the collider immediately so Dana can walk through the space 
        // while the breaking visual frames are still running.
        if (coll != null) coll.enabled = false;

        // Play the crumble animation state
        if (anim != null) anim.SetTrigger("Break");

        // Find out how long the break animation clip is, and destroy the object after it finishes
        float animationLength = GetAnimationClipLength("Block_Break");
        Destroy(gameObject, animationLength > 0 ? animationLength : 0.5f);
    }

    private float GetAnimationClipLength(string clipName)
    {
        if (anim == null) return 0f;
        foreach (var clip in anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName) return clip.length;
        }
        return 0f;
    }
}

