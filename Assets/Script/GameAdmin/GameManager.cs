using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

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
        Application.targetFrameRate = 60; //masih bisa diganti

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        resultScreenScript = FindFirstObjectByType<ResultScreenScript>(FindObjectsInactive.Include);
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
        //StartCoroutine(CountdownSound());
    }

    void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    void UnHideCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    bool startAudio = false;
    IEnumerator StartCountdown()
    {
        startAudio = false;

        while (countdown > 0)
        {
            int countdownInt = Mathf.CeilToInt(countdown);
            uiManager.CountdownHide();
            uiManager.countdownText.text = countdownInt.ToString();

            SoundManager.Instance.PlaySFXOnce(null, "StartSound");
            yield return new WaitForSeconds(3.9f);
            uiManager.CountdownShow();

            while (countdown > 0)
            {
                countdownInt = Mathf.CeilToInt(countdown);
                uiManager.countdownText.text = countdownInt.ToString();

                SoundManager.Instance.PlaySFXOnce(null, "LightCount");

                yield return new WaitForSeconds(1f);
                countdown -= 1f;
            }
        }

        // START
        uiManager.countdownText.text = "START";
        SoundManager.Instance.PlaySFXOnce(null, "LightStart");

        uiManager.HideCountdownTime();
        playerCarActive.enabled = true;

        gameStarted = true;
        countdown = 0f;
    }

    void GamePaused()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
            UnHideCursor();
        }
        else
        {
            Time.timeScale = 1f;
            HideCursor();
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
            float gameScoring = (carModel.carSpeed + scoreIncrease) * Time.deltaTime;
            gameScore += (int)gameScoring;
            gameLenght += carModel.carSpeed * Time.deltaTime;
        }
    }

    public void ScoreAdd(int scoreValue)
    {
        gameScore += scoreValue;
        Debug.Log("Score Tambah " + scoreValue);
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
            UnHideCursor();
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
        UpgradeLevel();
        ScoreCount();

        if (!gameOver)
        {
            GamePaused();
        }

        if (carModel.carSpeed > 0)
        {
            UIMonitor();
        }
    }
}
