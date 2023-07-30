using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private float maxHP = 3;
    [SerializeField] private Knockback knockback;
    [SerializeField] private Flash flash;
    [SerializeField] private GameObject deathVFX;

    [SerializeField] private bool shouldDestroOnDeath;
    [SerializeField] private float unconsciousTime = 60f;
    [SerializeField] private Animator animator;
    [SerializeField] private float DeathAnimationduration = 1f;

    private const string deathAnimationName = "Death";
    private const string idleAnimationName = "Idle";

    public event EventHandler OnEnemyHPReachZero;
    public event EventHandler OnEnemyHPRefreshed;

    private Action OnHPChange;

    private float currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float dmg, Vector3 damageSource, float knockbackPower)
    {
        currentHP = Mathf.Clamp(currentHP - dmg, 0, maxHP);
        knockback.GetknockedBack(damageSource, knockbackPower);
        OnHPChange?.Invoke();

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
        animator.SetBool("IsAttacking", false);
        animator.Play(deathAnimationName);
        if (shouldDestroOnDeath)
        {
            StartCoroutine(DestroyRoutine());
            return;
        }

        StartCoroutine(UnconsciousCoroutine());
    }

    private IEnumerator DestroyRoutine()
    {
        Instantiate(deathVFX, transform.position, Quaternion.identity);
        
        yield return new WaitForSeconds(DeathAnimationduration);
        Destroy(gameObject);
    }

    private IEnumerator UnconsciousCoroutine()
    {
        OnEnemyHPReachZero?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(unconsciousTime);

        currentHP = maxHP;
        OnHPChange?.Invoke();
        OnEnemyHPRefreshed?.Invoke(this, EventArgs.Empty);
        animator.Play(idleAnimationName);
    }

    public (float currentHP, float maxHP) GetHPInfo()
    {
        return (currentHP, maxHP);
    }

    public void SubscribeToHPChange(Action onHPChangeAction)
    {
        OnHPChange += onHPChangeAction;
    }

    public void UnsubscribeToHPChange(Action onHPChangeAction)
    {
        OnHPChange -= onHPChangeAction;
    }
}
