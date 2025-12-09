using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultScreenScript : MonoBehaviour
{
    public static ResultScreenScript instance;

    [Header("Component")]
    [SerializeField] TextMeshProUGUI finalScore;
    [SerializeField] TextMeshProUGUI finalLenght;

    [Header("GameScene")]
    [SerializeField] string gameScene;
    [SerializeField] string menuScene;

    [Header("Reference")]
    [SerializeField] GameManager gameManager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Start()
    {
        gameScene = SceneManager.GetActiveScene().name;
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameScene);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }

    public void SettingTheScore(int score, float lenght)
    {
        finalScore.text = score.ToString();
        finalLenght.text = lenght.ToString() + " M";
    }
}
