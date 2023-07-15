using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIMeleeAttack : EnemyAiAttackPattern
{
    public override void AttackBehaviour()
    {
        aiPath.enabled = true;
        aiDestinationSetter.enabled = true;
    }
}
