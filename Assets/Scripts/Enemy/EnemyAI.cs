using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyPathfinding enemyPathfinding;
    [SerializeField] private float romingDirectionRotation = 2f;
    [SerializeField] private float checkRadious = 10f;
    [SerializeField] private float attackRadious = 7f;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private AIPath aiPath;
    [SerializeField] private AIDestinationSetter aiDestinationSetter;
    [SerializeField] private AttackType attackType;
    [SerializeField] private bool drawGizmo = true;
    [SerializeField] private Knockback knockback;

    private Transform target;
    private State state;
    private bool isChaseRange;
    private bool isAttackRange;
    private IEnumerator roamingRoutine;
    private State previousState;

    private enum AttackType
    {
        GetCloser = 0,
        RangeAttack = 1
    }

    protected enum State
    {
        Roaming = 0,
        Chasing = 1,
        Attacking = 2, 
        KnockBack = 3
    }

    private void Awake()
    {
        state = State.Roaming;
        aiPath.enabled = false;
    }

    private void Start()
    {
        roamingRoutine = RoamingRoutine();
        StartCoroutine(roamingRoutine);
        aiDestinationSetter.target = PlayerController.Instance.transform;
        aiDestinationSetter.enabled = false;
        knockback.OnKnockbackStart += Knockback_OnKnockbackStart;
        knockback.OnKnockbackFinish += Knockback_OnKnockbackFinish;
    }

    private void Knockback_OnKnockbackFinish(object sender, System.EventArgs e)
    {
        GoToState(previousState);
    }

    private void Knockback_OnKnockbackStart(object sender, System.EventArgs e)
    {
        GoToState(State.KnockBack);
    }

    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, checkRadious);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadious);
        }
    }

    private void Update()
    {
        if (knockback.GettingKnockedBack)
        {
            return;
        }

        isChaseRange = Physics2D.OverlapCircle(transform.position, checkRadious, whatIsPlayer);
        isAttackRange = Physics2D.OverlapCircle(transform.position, attackRadious, whatIsPlayer);

        if (isAttackRange)
        {
            GoToState(State.Attacking);
            return;
        }
        if (isChaseRange)
        {
            GoToState(State.Chasing);
            return;
        }

        if(state != State.Roaming)
        {
            GoToState(State.Roaming);
        }
    }

    private IEnumerator RoamingRoutine()
    {
        while (state == State.Roaming)
        {
            Vector2 roamDirection = GetRoamingDirection();
            enemyPathfinding.SetMovementDirection(roamDirection);
            yield return new WaitForSeconds(romingDirectionRotation);
        }
    }

    private void GoToState(State desiredState)
    {
        previousState = state;
        state = desiredState;
        switch (desiredState)
        {
            case State.Roaming:
                aiPath.enabled = false;
                aiDestinationSetter.enabled = false;
                roamingRoutine = RoamingRoutine();
                StartCoroutine(roamingRoutine);
                break;
            case State.Chasing:
                StopCoroutine(roamingRoutine);
                aiPath.enabled = true;
                aiDestinationSetter.enabled = true;
                break;
            case State.Attacking:
                if (attackType == AttackType.GetCloser)
                {
                    StopCoroutine(roamingRoutine);
                    aiPath.enabled = true;
                    aiDestinationSetter.enabled = true;
                }
                else if (attackType == AttackType.RangeAttack)
                {
                    //to do
                }

                break;
            case State.KnockBack:
                aiPath.enabled = false;
                aiDestinationSetter.enabled = false;

                break;
            default:
                break;
        }
    }

    private Vector2 GetRoamingDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
