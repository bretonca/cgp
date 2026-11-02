using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class KeyPickup : MonoBehaviour
{

    public GameObject flyingKeyPrefab;
    public Transform doorTransform;
    private void OnTriggerEnter2D(Collider2D other)
    {
        DanaInventory inventory = other.GetComponent<DanaInventory>();

        if (inventory != null)
        {
            inventory.hasKey = true;

            GameObject key =
                Instantiate(
                    flyingKeyPrefab,
                    transform.position,
                    Quaternion.identity);

            key.GetComponent<FlyingKey>().targetDoor =
                doorTransform;

            Time.timeScale = 0f;

            Destroy(gameObject);
        }
    }
}
