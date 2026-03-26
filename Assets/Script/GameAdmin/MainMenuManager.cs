using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [Header("Window Screen")]
    [SerializeField] GameObject loadingScreen;

    [Header("Reference")]
    [SerializeField] OptionManager optionManager;

    //[Header("Referenece")]
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

        optionManager = FindFirstObjectByType<OptionManager>(FindObjectsInactive.Include);
    }

    void Start()
    {
        DontDestroyOnLoad(optionManager);
    }

    #region MainMenuButton

    public void GameStartButton()
    {
        //loadingScreen.SetActive(true);
        SceneManager.LoadScene("GameScene");
        //di loading screen, load scene game
    }

    public void OptionButton()
    {
        optionManager.EnableOption();
    }

    public void ExitButton() //keluar aplikasi
    {
        Application.Quit();
    }

    #endregion

    void Update()
    {
        
    }
}
