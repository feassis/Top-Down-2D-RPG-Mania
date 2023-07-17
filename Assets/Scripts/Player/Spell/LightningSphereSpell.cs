using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSphereSpell : Spell
{
    [SerializeField] private LightningSphere lightningSphere;
    [SerializeField] private List<LightningLevels> spellLevels;

    [Serializable]
    private struct LightningLevels
    {
        public float SpellCoolDown;
        public float Radius;
    }

    public override void CastSpell(int spellLevel)
    {
        var playerPos = PlayerController.Instance.transform.position;
        LightningSphere sphere = Instantiate(lightningSphere, playerPos, Quaternion.identity);
        sphere.transform.parent = PlayerController.Instance.transform;
        sphere.SetColliderRadious(spellLevels[spellLevel].Radius);
    }

    public override float GetCoolDownTine(int spellLevel)
    {
        return spellLevels[spellLevel].SpellCoolDown;
    }
}
