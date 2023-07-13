using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : MonoBehaviour
{
    [SerializeField] private List<Transform> path;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;

    private PathMove direction = PathMove.Fowards;
    private int currentPathIndex = 0;

    private enum PathMove
    {
        Fowards = 0,
        Backwards = 1
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, path[currentPathIndex].position) < 0.1f)
        {
            GoToNextIndex();
        }
    }

    private void FixedUpdate()
    {
        var moveDir = (path[currentPathIndex].transform.position - transform.position).normalized * speed * Time.fixedDeltaTime; 
        Vector3 movePosition =
             transform.position + new Vector3(moveDir.x, moveDir.y, 0);
        animator.SetFloat("MoveX", moveDir.x);
        animator.SetFloat("MoveY", moveDir.y);
        rb.MovePosition(movePosition);
    }

    private void GoToNextIndex()
    {
        if (direction == PathMove.Fowards)
        {
            currentPathIndex++;

            if(currentPathIndex >= path.Count)
            {
                currentPathIndex = path.Count - 2;
                direction = PathMove.Backwards;
            }
        }
        else
        {
            currentPathIndex--;
            if (currentPathIndex < 0)
            {
                currentPathIndex = 1;
                direction = PathMove.Fowards;
            }
        }
    }
}
