using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAiAttackPattern : MonoBehaviour
{
    [SerializeField] protected AIPath aiPath;
    [SerializeField] protected AIDestinationSetter aiDestinationSetter;
    public abstract void AttackBehaviour();
}
