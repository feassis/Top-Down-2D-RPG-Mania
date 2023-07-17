using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public SpellType SpellType;
    public string SpellName;

    public abstract void CastSpell(int spellLevel);

    public abstract float GetCoolDownTine(int spellLevel);
}

public enum SpellType
{
    Firebolt = 0,
    Heal = 1,
    Lighting = 2
}