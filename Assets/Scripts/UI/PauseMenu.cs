using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : Singleton<PauseMenu>
{
    [SerializeField] private Button resumeButtom;
    [SerializeField] private Button optionsButtom;
    [SerializeField] private Button quitButtom;
    [SerializeField] private GameObject panel;

    private PlayerControls controls;
    private bool isOpen;

    protected override void Awake()
    {
        resumeButtom.onClick.AddListener(OnResumeButtonClicked);
        optionsButtom.onClick.AddListener(OnOptionsButtonClicked);
        quitButtom.onClick.AddListener(OnQuitButtonClicked);

        CloseMenu();
        controls = new PlayerControls();
        controls.Menu.TogglePauseMenu.performed += TogglePauseMenu;
        base.Awake();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void TogglePauseMenu(InputAction.CallbackContext obj)
    {
        Debug.Log("toggle pause menu");
        if (isOpen)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    private void OnOptionsButtonClicked()
    {
        Debug.Log("To Be Defined");
    }

    private void OnResumeButtonClicked()
    {
        CloseMenu();
    }

    private void OpenMenu()
    {
        Time.timeScale = 0f;
        isOpen = true;
        panel.SetActive(true);
    }

    private void CloseMenu()
    {
        Time.timeScale = 1f;
        isOpen = false;
        panel.SetActive(false);
    }
}
