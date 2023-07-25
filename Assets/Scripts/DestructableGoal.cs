using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableGoal : MonoBehaviour
{
    public static event EventHandler OnAnyDestructableGoalDestroyed;
    void Start()
    {
        LevelDirector.Instance.RegisterDestructableGoal(this);
    }

    private void OnDestroy()
    {
        OnAnyDestructableGoalDestroyed?.Invoke(this, EventArgs.Empty);
    }
}
