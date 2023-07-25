using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachableGoal : MonoBehaviour
{
    private bool goalHasBeenReached = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (goalHasBeenReached)
        {
            return;
        }

        LevelDirector.Instance.GoalReached(this);
        goalHasBeenReached = true;
    }
}
