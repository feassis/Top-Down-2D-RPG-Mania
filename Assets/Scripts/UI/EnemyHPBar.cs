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
    }
}
