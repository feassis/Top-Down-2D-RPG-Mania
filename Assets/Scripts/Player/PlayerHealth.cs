using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Singleton<PlayerHealth>, IDamagable
{
    [SerializeField] private float maxHP = 10;
    [SerializeField] private Knockback knockback;
    [SerializeField] private Flash flash;

    private Action OnHPChange;

    private float currentHP;

    protected override void Awake()
    {
        base.Awake();
        currentHP = maxHP;
    }

    public void TakeDamage(float dmg, Vector3 damageSource, float knockbackPower)
    {
        currentHP = Mathf.Clamp(currentHP - dmg, 0, maxHP);
        knockback.GetknockedBack(damageSource, knockbackPower);
        OnHPChange?.Invoke();

        StartCoroutine(CheckDeathRoutine());
    }

    public void Heal(float amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
        OnHPChange?.Invoke();
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
        LevelDefeat.Instance.Open();
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
