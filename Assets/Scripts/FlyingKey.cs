using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlyingKey : MonoBehaviour
{
    public Transform targetDoor;
    public float speed = 5f;

    void Update()
    {
        if (targetDoor == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetDoor.position,
            speed * Time.unscaledDeltaTime
        );

        if (Vector3.Distance(transform.position, targetDoor.position) < 0.1f)
        {
            Door door = targetDoor.GetComponent<Door>();

            if (door != null)
            {
                door.UnlockDoor();
            }

            Time.timeScale = 1f;

            Destroy(gameObject);
        }
    }
}