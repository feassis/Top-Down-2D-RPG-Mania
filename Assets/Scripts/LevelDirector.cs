using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDirector : Singleton<LevelDirector>
{
    [SerializeField] private string nextLevelName;
    [SerializeField] private LevelCompletionRequirement levelCompletionRequirement;

    private List<DestructableGoal> destructableGoals = new List<DestructableGoal>();
    private List<ReachableGoal> reachableGoals = new List<ReachableGoal>();

    public event EventHandler<RemaingDestructables> OnDestructablesChanged;

    public class RemaingDestructables : EventArgs
    {
        public int RemainingTargets;
    }

    private enum LevelCompletionRequirement
    {
        ReachGoal = 0,
        TargetDestroyed = 1
    }

    public void RegisterDestructableGoal(DestructableGoal destructableGoal)
    {
        destructableGoals.Add(destructableGoal);
        OnDestructablesChanged?.Invoke(this, new RemaingDestructables { RemainingTargets = destructableGoals.Count});
    }

    public void RegisterReachableGoal(ReachableGoal reachableGoal)
    {
        if(levelCompletionRequirement != LevelCompletionRequirement.ReachGoal)
        {
            return;
        }

        reachableGoals.Add(reachableGoal);
    }

    public void GoalReached(ReachableGoal reachableGoal)
    {
        if (levelCompletionRequirement != LevelCompletionRequirement.ReachGoal)
        {
            return;
        }

        reachableGoals.Remove(reachableGoal);

        if(reachableGoals.Count <= 0)
        {
            LevelComplete.Instance.Open();
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        LevelTimer.Instance.StartTimer();
        if (levelCompletionRequirement == LevelCompletionRequirement.TargetDestroyed)
        {
            DestructableGoal.OnAnyDestructableGoalDestroyed += OnAnyDestructableGoal;
        }
    }

    private void OnAnyDestructableGoal(object sender, EventArgs e)
    {
        destructableGoals.Remove(sender as DestructableGoal);
        OnDestructablesChanged?.Invoke(this, new RemaingDestructables { RemainingTargets = destructableGoals.Count });
        if (destructableGoals.Count <= 0)
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

    public bool IsDestroableGoal()
    {
        return levelCompletionRequirement == LevelCompletionRequirement.TargetDestroyed;
    }

    public int RemainingTargets() => destructableGoals.Count;
}
