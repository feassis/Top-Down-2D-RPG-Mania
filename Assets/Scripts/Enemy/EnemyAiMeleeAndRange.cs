using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiMeleeAndRange : EnemyAiAttackPattern
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float spawnRange = 1f;

    private bool isAttackOnCooldown;

    public override void AttackBehaviour()
    {
        aiPath.enabled = true;
        aiDestinationSetter.enabled = true;

        if (isAttackOnCooldown)
        {
            return;
        }

        isAttackOnCooldown = true;
        Vector3 position = transform.position;
        var playerPos = PlayerController.Instance.transform.position;

        var direction = playerPos - position;

        Projectile projectileInstance =
            Instantiate(projectile, position + direction.normalized * spawnRange, Quaternion.identity);

        projectileInstance.ShootDirection = direction.normalized;

        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
    }
}
