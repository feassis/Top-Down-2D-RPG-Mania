using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireboltSpell : Spell
{
    [SerializeField] private Projectile firebolt;

    public override void CastSpell()
    {
        Vector3 position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        var playerPos = PlayerController.Instance.transform.position;

        var playerPositionOnScreen = Camera.main.WorldToViewportPoint(playerPos);

        var direction = position - playerPositionOnScreen;

        Projectile projectile = 
            Instantiate(firebolt, playerPos, Quaternion.identity);

        projectile.ShootDirection = direction.normalized;
    }
}
