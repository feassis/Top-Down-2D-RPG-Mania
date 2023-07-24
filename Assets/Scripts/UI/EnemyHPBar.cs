using System;
using UnityEngine;

public class EnemyHPBar : BaseHPBar
{
    [SerializeField] protected EnemyHealth enemyHealth;
    protected void Start()
    {
        damagable = (IDamagable)enemyHealth;
        damagable.SubscribeToHPChange(UpdateHPInfo);
        UpdateHPInfo();
        hpBackFillBar.fillAmount = barTarget;

        enemyHealth.OnEnemyHPReachZero += EnemyHeath_OnEnemyHPReachZero;
    }

    private void EnemyHeath_OnEnemyHPReachZero(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }
}
