using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class LevelComplete : Singleton<LevelComplete>
{
    [SerializeField] private List<StarLevels> StarLevelsThreshHold;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject bg;
    [SerializeField] private AudioSource buttonSound;

    private string nextLevelName;

    [System.Serializable]
    private struct StarLevels
    {
        public GameObject Star;
        public float Seconds;

        public void EvaluateStar(float levelDuration)
        {
            Star.SetActive(levelDuration <= Seconds);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
    }

    private void OnNextLevelButtonClicked()
    {
        buttonSound.Play();
        Close();
        SceneManager.LoadScene(nextLevelName);
        if (nextLevelName == "Main Menu")
            AkSoundEngine.SetState("Village_Combat", "None");
        else
            AkSoundEngine.SetState("Village_Combat", "Village");
    }

    public void Open()
    {
        AkSoundEngine.SetState("Village_Combat", "None");
        AkSoundEngine.SetState("Level_Game_Complete", "Level_Complete");
        AkSoundEngine.PostEvent("Play_Level_Game_Complete", gameObject);
        bg.SetActive(true);
        Time.timeScale = 0f;

        timerText.text = LevelTimer.Instance.GetTimerString();
        float levelDuration = LevelTimer.Instance.GetLevelDuration();

        nextLevelName = LevelDirector.Instance.GetNextLevelName();

        foreach (var star in StarLevelsThreshHold)
        {
            star.EvaluateStar(levelDuration);
        }
    }

    public void Close()
    {
        AkSoundEngine.PostEvent("Stop_Level_Game_Complete", gameObject);
        bg.SetActive(false);
        Time.timeScale = 1f;
    }
}
