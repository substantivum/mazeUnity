using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    public Button retryButton;
    [SerializeField]
    public Button menuButton;
    [SerializeField]
    public Button newGameButton;

    private GameManager gm;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        retryButton.onClick.AddListener(() => { gm.NewGame(true); });

        menuButton.onClick.AddListener(() => { SceneManager.LoadScene("Scenes/Menu");});

        newGameButton.onClick.AddListener(() => { gm.NewGame(false); });

    }
}

