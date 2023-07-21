using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpellPowerUp powerUpPrefab;

    private const string openAnimName = "ChestOpen";

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
        animator.Play(openAnimName);
        Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        this.enabled = false;
    }

    
}
