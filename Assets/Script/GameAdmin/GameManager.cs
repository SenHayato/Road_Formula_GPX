using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [Header("Game Information")]
    [SerializeField] int gameScore;
    [SerializeField] float gameLenght;
    [SerializeField] bool gameOver = false;
    public bool isPaused;

    [Header("Game Level Configuration")]
    [SerializeField] int startScore; //score awal sebagai tanda mulai
    [SerializeField] int[] gameScoreCap;
    [SerializeField] int scoreAddPerLevel;
    public int gameLevel;
    public int gameMaxLevel; //tambah 1 jika kondisi array diperlukan
    [SerializeField] int scoreIncrease;

    [Header("Start Countdown")]
    [SerializeField] float countdown = 0; //pastikan 0
    [SerializeField] bool gameStarted = false;
    [SerializeField] float countdownValue;

    [Header("Result Screen")]
    [SerializeField] GameObject resultScreen;

    [Header("Reference")]
    [SerializeField] UIManager uiManager;
    [SerializeField] CarModel carModel;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] ResultScreenScript resultScreenScript;
    [SerializeField] PlayerCarActive playerCarActive;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        resultScreenScript = FindFirstObjectByType<ResultScreenScript>();
        playerInput = GetComponent<PlayerInput>();
        carModel = FindFirstObjectByType<CarModel>();
        uiManager = FindFirstObjectByType<UIManager>();
        playerCarActive = FindFirstObjectByType<PlayerCarActive>();
    }

    void Start()
    {
        countdown = countdownValue;
        gameMaxLevel = gameScoreCap.Length;
        playerCarActive.enabled = false;
        resultScreen.SetActive(false);
        gameStarted = false;
        playerInput.enabled = true;

        ScoreSetting();
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        while (true)
        {
            int countdownInt = (int) countdown;
            if (countdown > 0)
            {
                countdown -= 1f * Time.deltaTime;
                uiManager.countdownText.text = countdownInt.ToString();

                if (countdown < 1f)
                {
                    uiManager.countdownText.text = "START";
                    uiManager.HideCountdown();
                    playerCarActive.enabled = true;

                    if (countdown <= 0)
                    {
                        gameStarted = true;
                        countdown = 0f;
                    }
                    //break;
                }
            }
            yield return null;
        }
    }

    void GamePaused()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    void ScoreSetting()
    {
        gameScoreCap[0] = startScore;
        for (int i = 1; i < gameMaxLevel; i++)
        {
            gameScoreCap[i] = gameScoreCap[i - 1] + (int)Mathf.Pow(scoreAddPerLevel, 1.37f);
        }
    }

    void UpgradeLevel()
    {
        int maxLevel = gameScoreCap.Length;
        for (int i = 0; i < maxLevel; i++)
        {
            if (gameScore >= gameScoreCap[i])
            {
                gameLevel = i + 1;
            }
        }

        gameLevel = Mathf.Clamp(gameLevel, 0, maxLevel);
    }

    void ScoreCount()
    {
        if (carModel.carSpeed > 0)
        {
            float gameScoring = (carModel.carSpeed + scoreIncrease) * Time.time;
            gameScore = (int)gameScoring;
            gameLenght += carModel.carSpeed * Time.deltaTime;
        }
    }

    void GameOver()
    {
        if ((carModel.carFuel <= 0f || playerCarActive.carExplode) && gameStarted)
        {
            gameOver = true;
            playerInput.enabled = false;
        }

        if (gameOver)
        {
            Invoke(nameof(ResultScreen), 5f);
        }
    }

    void ResultScreen()
    {
        Time.timeScale = 0f;
        resultScreen.SetActive(true);
        resultScreenScript.SettingTheScore(gameScore, gameLenght);
    }

    void UIMonitor()
    {
        uiManager.scoreText.text = gameScore.ToString();
        uiManager.lenghtText.text = gameLenght.ToString() + " M";
    }

    void Update()
    {
        GameOver();
        //UpgradeLevel();
        ScoreCount();

        if (!gameOver)
        {
            GamePaused();
        }

        if (carModel.carSpeed > 0)
        {
            UIMonitor();
        }

        //PlayBGM();
    }

    //test
    void PlayBGM() //untuk sound effect bisa memakai fungsi ini
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            MusicManager.Instance.MusicPlayList();
            //if (MusicManager.Instance != null)
            //{
                
            //}
        }
    }
}
