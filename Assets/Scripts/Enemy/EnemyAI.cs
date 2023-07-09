using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyPathfinding enemyPathfinding;
    [SerializeField] private float romingDirectionRotation = 2f;
    private State state;

    protected enum State
    {
        Roaming = 0
    }

    private void Awake()
    {
        state = State.Roaming;
    }

    private void Start()
    {
        StartCoroutine(RoamingRoutine());
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

    private Vector2 GetRoamingDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
