using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelDefeat : Singleton<LevelDefeat>
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private GameObject bg;
    [SerializeField] private AudioSource buttonSound;

    protected override void Awake()
    {
        base.Awake();
        mainMenuButton.onClick.AddListener(OnMenuButtonClicked);
        retryButton.onClick.AddListener(OnRetryClicked);
    }

    private void OnRetryClicked()
    {
        buttonSound.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AkSoundEngine.SetState("Level_Game_Complete", "None");
    }

    private void OnMenuButtonClicked()
    {
        buttonSound.Play();
        AkSoundEngine.SetState("Village_Combat", "None");
        SceneManager.LoadScene("MainMenu");
    }

    public void Open()
    {
        bg.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        bg.SetActive(false);
        Time.timeScale = 1f;
    }
}
