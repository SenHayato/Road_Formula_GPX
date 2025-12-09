using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [Header("Window Screen")]
    [SerializeField] GameObject optionScene;
    [SerializeField] GameObject loadingScreen;

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
    }

    void Start()
    {
        
    }

    #region MainMenuButton

    public void GameStartButton()
    {
        loadingScreen.SetActive(true);
        //di loading screen, load scene game
    }

    public void OptionButton()
    {
        optionScene.SetActive(true);
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
