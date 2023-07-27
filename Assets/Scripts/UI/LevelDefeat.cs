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

    protected override void Awake()
    {
        base.Awake();
        mainMenuButton.onClick.AddListener(OnMenuButtonClicked);
        retryButton.onClick.AddListener(OnRetryClicked);
    }

    private void OnRetryClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnMenuButtonClicked()
    {
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