using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : Spell
{
    [SerializeField] private GameObject healParticles;
    [SerializeField] private List<HealSpellLevels> spellLevels;

    [Serializable]
    private struct HealSpellLevels
    {
        public float HealAmount;
        public float SpellCoolDown;
    }

    public override void CastSpell(int spellLevel)
    {
        HealSpellLevels level = spellLevels[spellLevel];
        var playerHealth = PlayerHealth.Instance;

        playerHealth.Heal(level.HealAmount);

        Instantiate(healParticles, playerHealth.transform.position, Quaternion.identity);
    }

    public override float GetCoolDownTine(int spellLevel)
    {
        HealSpellLevels level = spellLevels[spellLevel];
        return level.SpellCoolDown;
    }
}
