using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimer : Singleton<LevelTimer>
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float timer;

    private bool isTimerOn = false;

    private void Update()
    {
        if (!isTimerOn)
        {
            return;
        }

        timer += Time.deltaTime;
        UpdateTimerText();
    }

    public void ResetTimer()
    {
        isTimerOn = false;
        timer = 0;
        UpdateTimerText();
    }

    public void StartTimer()
    {
        isTimerOn = true;
    }

    private void UpdateTimerText()
    {
        int seconds = Mathf.RoundToInt(timer);
        int hours = seconds / 3600;
        timerText.text = $"{hours} : { (seconds % 3600) / 60} : {(seconds % 3600) % 60}";
    }

    public string GetTimerString()
    {
        int seconds = Mathf.RoundToInt(timer);
        int hours = seconds / 3600;
        return $"{hours} : { (seconds % 3600) / 60} : {(seconds % 3600) % 60}";
    }

    public float GetLevelDuration()
    {
        return timer;
    } 
}
