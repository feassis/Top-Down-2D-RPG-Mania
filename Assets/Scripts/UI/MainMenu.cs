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

    private void Awake()
    {
        newGameButtom.onClick.AddListener(OnNewGameButtonClicked);
        optionsButtom.onClick.AddListener(OnOptionsButtonClicked);
        quitButtom.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    private void OnOptionsButtonClicked()
    {
        Debug.Log("TBD");
    }

    private void OnNewGameButtonClicked()
    {
        SceneManager.LoadScene(newGameSceneName);
    }
}
