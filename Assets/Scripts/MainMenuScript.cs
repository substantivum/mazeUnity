using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField]
    public Button playButton;
    [SerializeField]
    public Button quitButton;

    [SerializeField]
    public Button settingsButton;
    [SerializeField]
    public Button backSettingsButton;

    [SerializeField]
    public Button easyLevelButton;
    [SerializeField]
    public Button midLevelButton;
    [SerializeField]
    public Button hardLevelButton;

    [SerializeField]
    public GameObject mainMenu;
    [SerializeField]
    public GameObject levelsView;
    [SerializeField]
    public GameObject settingsView;


    private GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        playButton.onClick.AddListener(() => {
            mainMenu.SetActive(false);
            levelsView.SetActive(true);
                                                });

        settingsButton.onClick.AddListener(() =>
        {
            settingsView.SetActive(true);
            mainMenu.SetActive(false);
        });

        backSettingsButton.onClick.AddListener(() =>
        {
            settingsView.SetActive(false);
            mainMenu.SetActive(true);
        });

        quitButton.onClick.AddListener(() => { Application.Quit(); });

        easyLevelButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("Difficulty", (int)GameManager.Difficulty.Easy);
            SceneManager.LoadScene("Scenes/Game");
        });

        midLevelButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("Difficulty", (int)GameManager.Difficulty.Medium);
            SceneManager.LoadScene("Scenes/Game");
        });

        hardLevelButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("Difficulty", (int)GameManager.Difficulty.Hard);
            SceneManager.LoadScene("Scenes/Game");
        });

    }

    

}
