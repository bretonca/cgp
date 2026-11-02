using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private Animator anim;
    private bool opened = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void UnlockDoor()
    {
        if (opened) return;

        opened = true;
        anim.SetTrigger("OpenDoor");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DanaInventory inventory = other.GetComponent<DanaInventory>();

        if (inventory == null)
            return;

        // If door is closed and player has key
        if (!opened && inventory.hasKey)
        {
            opened = true;

            anim.SetTrigger("OpenDoor");

            inventory.hasKey = false;
        }
        // If door is already open, enter the level
        else if (opened)
        {
            Debug.Log("Continue to Next Level");

            NextLevel();
        }
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex + 1
        );
    }
}