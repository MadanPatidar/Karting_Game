using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameManager : MonoBehaviour
{
    public Button pauseButton;
    public PauseScreen pauseScreen;

    [Tooltip("Master volume when menu is open")]
    [Range(0.001f, 1f)]
    public float volumeWhenMenuOpen = 0.5f;

    void Start()
    {
        pauseButton.onClick.AddListener(PauseButtonOnTap);
    }

    void PauseButtonOnTap()
    {
        pauseScreen.gameObject.SetActive(true);
        Time.timeScale = 0f;
        AudioUtility.SetMasterVolume(volumeWhenMenuOpen);
        //  EventSystem.current.SetSelectedGameObject(null);        
    }
}