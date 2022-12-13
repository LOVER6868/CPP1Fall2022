using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class CanvasManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Button")]
    public Button startButton;
    public Button settingsButton;
    public Button quitButton;
    public Button returnToMenuButton;
    public Button returnToGameButton;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject pauseMenu;

    [Header("Text")]
    public Text livesText;
    public Text MusicSliderText;
    public Text SFXSliderText;

    [Header("Slidedr:")]
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    void ShowSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    void OnMusicSliderValueChanged(float value)
    {
        if (MusicSliderText)
        {
            MusicSliderText.text = value.ToString();
            audioMixer.SetFloat("MusicVol", value - 80);
        }
    }
    void OnSFXSliderValueChanged(float value)
    {
        if (SFXSliderText)
        {
            SFXSliderText.text = value.ToString();
            audioMixer.SetFloat("SFXVol", value - 80);
        }
    }


    void QuitGame()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }

    void ShowMainMenu()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            SceneManager.LoadScene(0);
            ResumeGame();
        }
        else {
            mainMenu.SetActive(true);
            settingsMenu.SetActive(false);
        }
    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        GameManager.instance.paused = false;
    }

    void UpdateLifeText(int value)
    {
        livesText.text = value.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (startButton)
            startButton.onClick.AddListener(StartGame);

        if (settingsButton)
            settingsButton.onClick.AddListener(ShowSettingsMenu);

        if (quitButton)
            quitButton.onClick.AddListener(QuitGame);

        if (returnToMenuButton)
            returnToMenuButton.onClick.AddListener(ShowMainMenu);

        if (musicVolSlider)
        {
            musicVolSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            float mixerValue;
            audioMixer.GetFloat("MusicVol", out mixerValue);
            musicVolSlider.value = mixerValue + 80;
        }

        if (sfxVolSlider)
        {
            sfxVolSlider.onValueChanged.AddListener(OnSFXSliderValueChanged);
            float mixerValue;
            audioMixer.GetFloat("SFXVol", out mixerValue);
            sfxVolSlider.value = mixerValue + 80;
        }

        if (returnToGameButton)
            returnToGameButton.onClick.AddListener(ResumeGame);

        if (livesText)
            GameManager.instance.onLifeValueChanged.AddListener(UpdateLifeText);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMenu)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                pauseMenu.SetActive(!pauseMenu.activeSelf);

                if (pauseMenu.activeSelf)
                {
                    Time.timeScale = 0;
                    GameManager.instance.paused = true;
                }
                else
                {
                    Time.timeScale = 1;
                    GameManager.instance.paused = false;
                }
            }
        }
    }
}
