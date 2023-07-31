using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private string nextSceneName;
    [SerializeField] private Button nextSceneButton;

    private void Awake()
    {
        nextSceneButton.onClick.AddListener(OnNextSceneClicked);
    }

    private void OnNextSceneClicked()
    {
        AkSoundEngine.SetState("Village_Combat", "Village");
        AkSoundEngine.PostEvent("Play_Village_Combat", gameObject);
        SceneManager.LoadScene(nextSceneName);
    }
}
