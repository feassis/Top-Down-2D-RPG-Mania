using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TargetUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainingTargetsText;
    void Start()
    {
        remainingTargetsText.gameObject.SetActive(false);
        StartCoroutine(StartUpCoroutine());
    }

    private IEnumerator StartUpCoroutine()
    {
        yield return null;
        SetupText();
        LevelDirector.Instance.OnDestructablesChanged += LevelDirector_OnDestructableGoalChanged;
    }

    private void OnDestroy()
    {
        LevelDirector.Instance.OnDestructablesChanged -= LevelDirector_OnDestructableGoalChanged;
    }

    private void LevelDirector_OnDestructableGoalChanged(object sender, LevelDirector.RemaingDestructables e)
    {
        remainingTargetsText.text = $"Remaining Targets: {e.RemainingTargets}";
    }

    private void SetupText()
    {
        bool isDestructableTarget = LevelDirector.Instance.IsDestroableGoal();

        if (!isDestructableTarget)
        {
            remainingTargetsText.gameObject.SetActive(false);
            return;
        }

        remainingTargetsText.gameObject.SetActive(true);

        int targetAmount = LevelDirector.Instance.RemainingTargets();
        remainingTargetsText.text = $"Remaining Targets: {targetAmount}";
    }
}
