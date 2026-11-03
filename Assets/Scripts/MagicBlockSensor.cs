using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MagicBlockSensor : MonoBehaviour
{
    public BoxCollider2D magicBlockSensor;

    public bool isTouchingWall;

    void Update()
    {
        isTouchingWall = magicBlockSensor.IsTouchingLayers(
            LayerMask.GetMask("Ground", "MagicBlock")
        );

        Debug.Log(isTouchingWall);
    }
}
