using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public static PauseScript Instance;

    [Header("Pause Screen")]
    [SerializeField] GameObject pauseScreen;

    [Header("Scene Properties")]
    [SerializeField] string thisSceneName;
    [SerializeField] string mainMenuScene;

    [Header("Reference")]
    [SerializeField] InputScript inputScript;
    [SerializeField] GameManager gameManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        thisSceneName = SceneManager.GetActiveScene().name;

        inputScript = FindFirstObjectByType<InputScript>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void PauseButton()
    {
        if (inputScript.pauseAction.triggered)
        {
            if (gameManager.isPaused)
            {
                gameManager.isPaused = false;
            }
            else
            {
                gameManager.isPaused = true;
            }
        }
    }

    void PauseScreenMonitor()
    {
        if (!gameManager.isPaused)
        {
            pauseScreen.SetActive(false);
        }
        else
        {
            pauseScreen.SetActive(true);
        }
    }

    #region PauseScreenButton
    //Setting tombol dari pause screen

    public void ContinueButton()
    {
        gameManager.isPaused = false;
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(thisSceneName);
    }

    public void OptionButton()
    {
        //munculkan option screen
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    #endregion

    private void Update()
    {
        PauseButton();
        PauseScreenMonitor();
    }
}
