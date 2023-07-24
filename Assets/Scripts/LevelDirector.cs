using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDirector : Singleton<LevelDirector>
{
    [SerializeField] private string nextLevelName;
    [SerializeField] private LevelCompletionRequirement levelCompletionRequirement;

    private List<DestructableGoal> destructableGoals = new List<DestructableGoal>();

    private enum LevelCompletionRequirement
    {
        ReachGoal = 0,
        TargetDestroyed = 1
    }

    internal void RegisterDestructableGoal(DestructableGoal destructableGoal)
    {
        destructableGoals.Add(destructableGoal);
    }

    protected override void Awake()
    {
        base.Awake();
        LevelTimer.Instance.StartTimer();
    }

    private void Start()
    {
        if(levelCompletionRequirement == LevelCompletionRequirement.TargetDestroyed)
        {
            DestructableGoal.OnAnyDestructableGoalDestroyed += OnAnyDestructableGoal;
        }
    }

    private void OnAnyDestructableGoal(object sender, EventArgs e)
    {
        destructableGoals.Remove(sender as DestructableGoal);

        if(destructableGoals.Count <= 0)
        {
            LevelComplete.Instance.Open();
        }
    }

    protected override void OnDestroy()
    {
        LevelTimer.Instance.ResetTimer();
        base.OnDestroy();
    }

    public string GetNextLevelName()
    {
        return nextLevelName;
    }
}
