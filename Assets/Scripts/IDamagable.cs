using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void SubscribeToHPChange(Action onHPChangeAction);
    void UnsubscribeToHPChange(Action onHPChangeAction);

    void TakeDamage(float dmg, Vector3 damageSource, float knockbackPower);
    (float currentHP, float maxHP) GetHPInfo();
}
