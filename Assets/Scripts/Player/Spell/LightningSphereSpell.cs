using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSphereSpell : Spell
{
    [SerializeField] private LightningSphere lightningSphere;
    public override void CastSpell()
    {
        var playerPos = PlayerController.Instance.transform.position;
        Instantiate(lightningSphere, playerPos, Quaternion.identity);
    }
}
