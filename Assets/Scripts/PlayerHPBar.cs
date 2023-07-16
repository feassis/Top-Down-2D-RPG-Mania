using System.Collections;
using System.Collections.Generic;

public class PlayerHPBar : BaseHPBar
{
    protected PlayerHealth playerHealth;
    protected void Start()
    {
        playerHealth = PlayerHealth.Instance;
        damagable = (IDamagable) playerHealth;
        damagable.SubscribeToHPChange(UpdateHPInfo);
        UpdateHPInfo();
        hpBackFillBar.fillAmount = barTarget;
    }
}
