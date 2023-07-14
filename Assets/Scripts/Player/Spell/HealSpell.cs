using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : Spell
{
    [SerializeField] private float healAmount;
    [SerializeField] private GameObject healParticles;

    public override void CastSpell()
    {
        var playerHealth = PlayerHealth.Instance;

        playerHealth.Heal(healAmount);

        Instantiate(healParticles, playerHealth.transform.position, Quaternion.identity);
    }
}
