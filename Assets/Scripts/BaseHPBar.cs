using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseHPBar : MonoBehaviour
{
    [SerializeField] protected Image hpFillBar;
    [SerializeField] protected Image hpBackFillBar;
    [SerializeField] private float hpBarSpeed = 0.5f;
    [SerializeField] protected IDamagable damagable;

    protected float barTarget;

    private void Update()
    {
        hpBackFillBar.fillAmount = Mathf.Lerp(hpBackFillBar.fillAmount, 
            barTarget, hpBarSpeed * Time.deltaTime);
    }

    protected void UpdateHPInfo()
    {
        var hpInfo = damagable.GetHPInfo();

        barTarget = hpInfo.currentHP / hpInfo.maxHP;
        hpFillBar.fillAmount = barTarget;
    }
}
