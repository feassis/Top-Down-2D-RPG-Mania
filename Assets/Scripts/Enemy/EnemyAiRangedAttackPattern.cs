using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiRangedAttackPattern : EnemyAiAttackPattern
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private float attackCooldown = 1f;

    private bool isAttackOnCooldown;

    public override void AttackBehaviour()
    {
        if (isAttackOnCooldown)
        {
            return;
        }

        aiPath.enabled = false;
        aiDestinationSetter.enabled = false;

        isAttackOnCooldown = true;
        Vector3 position = transform.position;
        var playerPos = PlayerController.Instance.transform.position;

        var direction = playerPos - position;

        Projectile projectileInstance =
            Instantiate(projectile, position, Quaternion.identity);

        projectileInstance.ShootDirection = direction.normalized;

        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
    }
}
