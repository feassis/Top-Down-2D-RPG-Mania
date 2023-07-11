using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public string SpellName;
    public float SpellCoolDown;

    public abstract void CastSpell();
}
