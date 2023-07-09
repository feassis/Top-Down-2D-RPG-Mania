using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private float damageAmount = 1;
    [SerializeField] private float knockbackPoweronEnemy = 15f;
    [SerializeField] private float knockbackPowerOnPlayer = 15f;
    [SerializeField] private bool shouldKnokbackPlayer;
    [SerializeField] private Transform knockbackSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            if (shouldKnokbackPlayer)
            {
                PlayerController.Instance.KnockbackPlayer(knockbackSource.position, knockbackPowerOnPlayer);
            }

            enemyHealth.TakeDamage(damageAmount, knockbackSource.position, knockbackPoweronEnemy);
        }
    }
}
