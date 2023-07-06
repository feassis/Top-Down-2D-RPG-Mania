using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Knockback knockback;
    private Vector3 movementDirection;

  

    public void SetMovementDirection(Vector2 moveDir)
    {
        movementDirection = moveDir;
    }

    private void Move()
    {
        if (knockback.GettingKnockedBack)
        {
            return;
        }

        Vector2 moveDir = movementDirection.normalized * speed * Time.fixedDeltaTime;
        Vector3 movePosition =
            transform.position + new Vector3(moveDir.x, moveDir.y, 0);
        rb.MovePosition(movePosition);
    }

    private void FixedUpdate()
    {
        Move();
    }
}
