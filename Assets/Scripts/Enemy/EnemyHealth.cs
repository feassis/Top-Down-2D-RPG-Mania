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
    [SerializeField] private GameObject zzzObject;
    [SerializeField] private float unconsciousTime = 60f;

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
        if (shouldDestroOnDeath)
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }

        StartCoroutine(UnconsciousCoroutine());
    }

    private IEnumerator UnconsciousCoroutine()
    {
        OnEnemyHPReachZero?.Invoke(this, EventArgs.Empty);

        zzzObject.SetActive(true);

        yield return new WaitForSeconds(unconsciousTime);

        currentHP = maxHP;
        OnHPChange?.Invoke();

        zzzObject.SetActive(false);
        OnEnemyHPRefreshed?.Invoke(this, EventArgs.Empty);
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
