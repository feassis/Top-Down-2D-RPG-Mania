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
    [SerializeField] private EnemyAiAttackPattern enemyAIAttackPattern;
    [SerializeField] private EnemyHPBar hpBar;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private DamageSource damageSource;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;

    private Transform target;
    private State state;
    private bool isChaseRange;
    private bool isAttackRange;
    private IEnumerator roamingRoutine;
    private State previousState;
    private Vector3 targetDirection;

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
        KnockBack = 3,
        Stunned = 4
    }

    private void Awake()
    {
        state = State.Roaming;
        aiPath.enabled = false;
    }

    private void Start()
    {
        hpBar.gameObject.SetActive(true);
        roamingRoutine = RoamingRoutine();
        StartCoroutine(roamingRoutine);
        target = PlayerController.Instance.transform;
        aiDestinationSetter.target = target;
        aiDestinationSetter.enabled = false;
        knockback.OnKnockbackStart += Knockback_OnKnockbackStart;
        knockback.OnKnockbackFinish += Knockback_OnKnockbackFinish;
        enemyHealth.OnEnemyHPReachZero += EnemyHealth_OnEnemyHPReachZero;
        enemyHealth.OnEnemyHPRefreshed += EnemyHealth_OnEnemyHPRefreshed;
    }

    private void EnemyHealth_OnEnemyHPRefreshed(object sender, System.EventArgs e)
    {
        damageSource.ToggleDamageSource(true);
        rb.isKinematic = false;
        GoToState(State.Roaming);
    }

    private void EnemyHealth_OnEnemyHPReachZero(object sender, System.EventArgs e)
    {
        GoToState(State.Stunned);
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
        if(state == State.Stunned)
        {
            return;
        }

        if(state == State.Roaming)
        {
            hpBar.gameObject.SetActive(false);
        }

        if (knockback.GettingKnockedBack)
        {
            return;
        }

        isChaseRange = Physics2D.OverlapCircle(transform.position, checkRadious, whatIsPlayer);
        isAttackRange = Physics2D.OverlapCircle(transform.position, attackRadious, whatIsPlayer);

        targetDirection = (target.transform.position - transform.position).normalized;
        animator.SetFloat("Speed", targetDirection.magnitude);
        animator.SetBool("IsAttacking", isAttackRange);
        sprite.flipX = targetDirection.x < 0;

        if (isAttackRange)
        {
            hpBar.gameObject.SetActive(true);
            GoToState(State.Attacking);
            return;
        }
        if (isChaseRange)
        {
            hpBar.gameObject.SetActive(true);
            GoToState(State.Chasing);
            return;
        }

        if(state != State.Roaming)
        {
            hpBar.gameObject.SetActive(false);
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
                AkSoundEngine.SetState("Village_Combat", "Village"); //Set Music State to Village Music
                aiPath.enabled = false;
                aiDestinationSetter.enabled = false;
                enemyPathfinding.enabled = true;
                roamingRoutine = RoamingRoutine();
                StartCoroutine(roamingRoutine);
                break;
            case State.Chasing:
                if (gameObject.name == "Boss") //Set Music State to Combat Music depending on if enemy is normal or Chief.
                    AkSoundEngine.SetState("Village_Combat", "Combat_Final"); 
                else
                    AkSoundEngine.SetState("Village_Combat", "Combat_Normal"); 
                StopCoroutine(roamingRoutine);
                enemyPathfinding.enabled = false;
                aiPath.enabled = true;
                aiDestinationSetter.enabled = true;
                break;
            case State.Attacking:
                StopCoroutine(roamingRoutine);
                enemyPathfinding.enabled = false;

                enemyAIAttackPattern.AttackBehaviour();

                break;
            case State.KnockBack:
                aiPath.enabled = false;
                aiDestinationSetter.enabled = false;
                animator.SetFloat("Speed", 0);

                break;
            case State.Stunned:
                StopCoroutine(roamingRoutine);
                knockback.StopKnockBack();
                aiDestinationSetter.target = null;
                enemyPathfinding.enabled = false;
                aiPath.enabled = false;
                aiDestinationSetter.enabled = false;
                damageSource.ToggleDamageSource(false);
                rb.isKinematic = true;
                animator.SetFloat("Speed", 0);
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
