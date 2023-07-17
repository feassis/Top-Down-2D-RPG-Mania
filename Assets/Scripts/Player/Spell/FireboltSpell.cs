using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireboltSpell : Spell
{
    [SerializeField] private Projectile firebolt;
    [SerializeField] private float instantiationDistance = 1f;
    [SerializeField] private List<FireboltLevels> spellLevels;

    [Serializable]
    private struct FireboltLevels
    {
        public int ProjectileAmount;
        public float TimeBetween;
        public float SpellCoolDown;
    }

    public override void CastSpell(int spellLevel)
    {
        StartCoroutine(ShootProjectile(spellLevel));
    }

    private IEnumerator ShootProjectile(int spellLevel)
    {
        FireboltLevels level = spellLevels[spellLevel];
        for (int i = 0; i < level.ProjectileAmount; i++)
        {
            Vector3 position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            var playerPos = PlayerController.Instance.transform.position;

            var playerPositionOnScreen = Camera.main.WorldToViewportPoint(playerPos);

            var direction = position - playerPositionOnScreen;

            Projectile projectile =
                Instantiate(firebolt, playerPos + direction.normalized * instantiationDistance, Quaternion.identity);

            projectile.ShootDirection = direction.normalized;

            yield return new WaitForSeconds(level.TimeBetween);
        }     
    }

    public override float GetCoolDownTine(int spellLevel)
    {
        return spellLevels[spellLevel].SpellCoolDown;
    }
}
