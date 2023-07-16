using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDirector : MonoBehaviour
{
    private float seconds;

    private void Awake()
    {
        LevelTimer.Instance.StartTimer();
    }

    private void OnDestroy()
    {
        LevelTimer.Instance.ResetTimer();
    }
}
