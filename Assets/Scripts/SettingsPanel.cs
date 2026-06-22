using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SettingsPanel : MonoBehaviour
{
    public static bool isOpen = false;

    public Slider volumeSlider;
    public GameObject settingsPanel;
    public string mainMenuScene = "EscenaInicio";

    void Start()
    {
        if (volumeSlider != null)
        {
            float saved = PlayerPrefs.GetFloat("MasterVolume", 1f);
            volumeSlider.value = saved;
            ApplyVolume(saved);
            volumeSlider.onValueChanged.AddListener(ApplyVolume);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePanel();
    }

    void ApplyVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public float pauseDelay = 0.5f;
    public float resumeDelay = 1.5f;

    public void TogglePanel()
    {
        if (settingsPanel == null) return;
        if (settingsPanel.activeSelf)
            ClosePanel();
        else
            OpenPanel();
    }

    public void OpenPanel()
    {
        if (settingsPanel == null) return;
        StopAllCoroutines();
        settingsPanel.SetActive(true);
        isOpen = true;
        UIBlocker.Open();
        StartCoroutine(PauseWithDelay());
    }

    public void ClosePanel()
    {
        if (settingsPanel == null) return;
        settingsPanel.SetActive(false);
        UIBlocker.Close();
        StartCoroutine(ResumeWithDelay());
    }

    IEnumerator PauseWithDelay()
    {
        yield return new WaitForSecondsRealtime(pauseDelay);
        Time.timeScale = 0f;
    }

    IEnumerator ResumeWithDelay()
    {
        yield return new WaitForSecondsRealtime(resumeDelay);
        isOpen = false;
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
