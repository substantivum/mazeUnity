using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCamera;

    [SerializeField]
    private bool useSeed;

    [SerializeField]
    private TextMeshProUGUI gameStatusText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI coinsText;

    private MazeGenerator mazeGenerator;

    [SerializeField]
    public TextMeshProUGUI timeLeftText;

    [SerializeField]
    public GameObject buttons;

    private PlayerController player;

    public float maxTime = 30;
    public float timeRemaining = 30f;
    public float timeScoreFactor = 100f;
    private float currentTime;
    private float initialTime;

    public int coinsCollected = 0;

    private Animator animator;

    public int levelDiff;

    private bool gameOver = false;

    public enum timeDifficulty { EASY, MEDIUM, HARD }
    public enum Difficulty { Easy, Medium, Hard }
    public Difficulty CurrentDifficulty { get; private set; }

    private int bestScore;

    private MazeSettings easySettings = new MazeSettings(10, 10, 1, new Vector3(5, 10, 4.5f));
    private MazeSettings mediumSettings = new MazeSettings(15, 15, 2, new Vector3(6.5f, 15, 7));
    private MazeSettings hardSettings = new MazeSettings(20, 20, 3, new Vector3(9.5f, 20, 9.5f));




    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        mazeGenerator = FindObjectOfType<MazeGenerator>();
        animator = player.GetComponentInChildren<Animator>();
        
        
        LoadDifficulty();
        setDifficulty(CurrentDifficulty);
        player.enabled = true;
        NewGame(false);

    }

    private void Update()
    {
        if (!gameOver)
        {
            UpdateTime();
            UpdateScore();

            coinsText.SetText("Coins collected: " + coinsCollected + "/3");
        }
    }

    private void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
    }
    private void LoadDifficulty()
    {
        int savedDifficulty = PlayerPrefs.GetInt("Difficulty", (int)Difficulty.Easy);
        CurrentDifficulty = (Difficulty)savedDifficulty;
        maxTime = PlayerPrefs.GetFloat("TimeLevel", 30f);
        Debug.Log("Loaded difficulty: " + CurrentDifficulty);
    }
   

    public void setDifficulty(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
        print(difficulty);
    }

    private void SaveBestScore()
    {
        PlayerPrefs.SetFloat("BestScore", bestScore);
        PlayerPrefs.Save();
    }
    private void UpdateScore()
    {
        currentTime = maxTime - (Time.time - initialTime);
        int totalTimeScore = Mathf.RoundToInt(timeRemaining * 10);
        int coinScore = coinsCollected * 100;

        float totalScore = Mathf.Clamp(currentTime * timeScoreFactor, 0f, Mathf.Infinity) + coinScore;
        
        scoreText.SetText("Total Score: " + Mathf.RoundToInt(totalScore).ToString());

        if (totalScore > bestScore)
        {
            bestScore = Mathf.RoundToInt(totalScore);
            SaveBestScore();
            scoreText.SetText("New best score: " + bestScore.ToString());
        }
    }

    public MazeSettings GetCurrentMazeSettings()
    {
        switch (CurrentDifficulty)
        {
            case Difficulty.Medium:
                return mediumSettings;
            case Difficulty.Hard:
                return hardSettings;
            default:
                return easySettings;
        }
    }

    void StartTimer()
    {
        print("set timer maxtime" + maxTime);
        timeRemaining = maxTime;
        UpdateText();

    }

    private void UpdateTime()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(0f, timeRemaining);
            UpdateText();
        }
        else
        {

            timeLeftText.text = "00:00";
            GameOver(true);
        }
    }
    private void UpdateText()
    {
        string formattedTime = $"{Mathf.FloorToInt(timeRemaining / 60):00}:{Mathf.FloorToInt(timeRemaining % 60):00}";
        timeLeftText.text = formattedTime;
    }

    public void NewGame(bool useSeed)
    {

        gameOver = false;
        if (mazeGenerator.mazeParent != null)
        {
            Destroy(mazeGenerator.mazeParent);
        }

        player.enabled = true;
        animator.SetBool("Run_b", true);
        buttons.SetActive(false);
        gameStatusText.gameObject.SetActive(false);

        coinsCollected = 0;
        StartTimer();
        print("use seed: " + useSeed);
        mazeGenerator.CreateMaze(useSeed, GetCurrentMazeSettings());
        print(GetCurrentMazeSettings());
        scoreText.gameObject.SetActive(false);

    }

    public void GameOver(bool isLost)
    {
        if (isLost)
        {
            gameStatusText.text = "YOU LOST";
            gameStatusText.gameObject.SetActive(true);
            buttons.SetActive(true);
            player.enabled = false;
            gameOver = true;
            scoreText.gameObject.SetActive(false);

        }

        else
        {
            gameStatusText.text = "VICTORY";
            gameStatusText.gameObject.SetActive(true);
            buttons.SetActive(true);
            player.enabled = false;
            animator.SetBool("Run_b", false);
            gameOver = true;
            scoreText.gameObject.SetActive(true);
            animator.SetBool("Victory_b", true);
        }

    }
}
