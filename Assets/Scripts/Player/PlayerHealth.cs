using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHP = 10;
    [SerializeField] private Knockback knockback;
    [SerializeField] private Flash flash;

    private float currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float dmg, Vector3 damageSource, float knockbackPower)
    {
        currentHP = Mathf.Clamp(currentHP - dmg, 0, maxHP);
        knockback.GetknockedBack(damageSource, knockbackPower);

        StartCoroutine(CheckDeathRoutine());
    }

    private IEnumerator CheckDeathRoutine()
    {
        yield return StartCoroutine(flash.FlashRoutine());

        if (currentHP <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        
    }
}
