using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public Button resumeButton;
    public Button menuButton;

    string SceneName = "IntroMenu";
    void Start()
    {
        resumeButton.onClick.AddListener(ResumeButtonOnTap);
        menuButton.onClick.AddListener(MenuButtonOnTap);
    }

    void MenuButtonOnTap()
    {
        Time.timeScale = 1f;
        AudioUtility.SetMasterVolume(1);
        SceneManager.LoadSceneAsync(SceneName);
        gameObject.SetActive(false);
    }

    void ResumeButtonOnTap()
    {
        Time.timeScale = 1f;
        AudioUtility.SetMasterVolume(1);
        gameObject.SetActive(false);
    }
}