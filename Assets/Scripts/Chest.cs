using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpellPowerUp powerUpPrefab;
    [SerializeField] private AudioSource chestSound;

    private const string openAnimName = "ChestOpen";

    private bool alreadyOpened;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.AddInteractable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.RemoveInteractable(this);
        }
    }

    public void Interact()
    {
        if (alreadyOpened)
        {
            return;
        }

        animator.Play(openAnimName);
        chestSound.Play();
        Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        this.enabled = false;
        alreadyOpened = true;
    }

    
}
