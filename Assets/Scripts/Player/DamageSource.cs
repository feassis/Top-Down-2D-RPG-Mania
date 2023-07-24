using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private float damageAmount = 1;
    [SerializeField] private float knockbackPowerOnTarget = 15f;
    [SerializeField] private float knockbackPowerOnSelf = 15f;
    [SerializeField] private bool shouldKnokbackSelf;
    [SerializeField] private Transform knockbackSource;
    [SerializeField] protected bool isPlayer = true;

    private bool isOn = true; 
    public void ToggleDamageSource(bool toggle)
    {
        isOn = toggle;
    }

    public event EventHandler<CollisionEventHandler> OnObstacleHited;

    public class CollisionEventHandler : EventArgs
    {
        public Collider2D Collision;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOn)
        {
            return;
        }

        if (isPlayer)
        {
            DamageAsPlayer(collision);
        }
        else
        {
            DamageAsEnemy(collision);
        }

        OnObstacleHited?.Invoke(this, new CollisionEventHandler { Collision = collision }) ;
    }

    private void DamageAsEnemy(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            if (shouldKnokbackSelf)
            {
                if(gameObject.TryGetComponent<Knockback>(out Knockback knockback))
                {
                    knockback.GetknockedBack(playerHealth.transform.position, knockbackPowerOnSelf);
                }
            }

            playerHealth.TakeDamage(damageAmount, knockbackSource.position, knockbackPowerOnTarget);
        }
    }

    private void DamageAsPlayer(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            if (shouldKnokbackSelf)
            {
                PlayerController.Instance.KnockbackPlayer(knockbackSource.position, knockbackPowerOnSelf);
            }

            enemyHealth.TakeDamage(damageAmount, knockbackSource.position, knockbackPowerOnTarget);
        }
    }
}
