using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedEnemy : MonoBehaviour
{
    public BoxCollider2D leftGroundCheck;
    public BoxCollider2D rightGroundCheck;
    public BoxCollider2D feet;
    public BoxCollider2D magicBlockSensor;
    public BoxCollider2D leftWallCheck;
    public BoxCollider2D rightWallCheck;

    protected static Rigidbody2D rb;

    public float speed = 1;
    public bool facingRight = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
