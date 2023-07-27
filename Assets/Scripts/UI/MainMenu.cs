using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button newGameButtom;
    [SerializeField] private Button optionsButtom;
    [SerializeField] private Button quitButtom;
    [SerializeField] private string newGameSceneName = "Scene 1";
    [SerializeField] private AudioSource buttonSound;

    private void Awake()
    {
        newGameButtom.onClick.AddListener(OnNewGameButtonClicked);
        optionsButtom.onClick.AddListener(OnOptionsButtonClicked);
        quitButtom.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnQuitButtonClicked()
    {
        buttonSound.Play();
        Application.Quit();
    }

    private void OnOptionsButtonClicked()
    {
        buttonSound.Play();
        Debug.Log("TBD");
    }

    private void OnNewGameButtonClicked()
    {
        buttonSound.Play();
        SceneManager.LoadScene(newGameSceneName);
    }
}
