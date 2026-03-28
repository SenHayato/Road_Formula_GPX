using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [Header("Window Screen")]
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Animation mainMenuAnimation;
    [SerializeField] Animation titleMenuAnimation;

    [Header("Reference")]
    [SerializeField] OptionManager optionManager;
    [SerializeField] PlayerInput playerInput;
    private InputAction anyKeyAction;

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
        playerInput = FindFirstObjectByType<PlayerInput>();
        anyKeyAction = playerInput.actions.FindAction("Anykey");
    }

    void Start()
    {
        DontDestroyOnLoad(optionManager);
        StartCoroutine(OnAnykey());
    }

    IEnumerator OnAnykey()
    {
        while (true)
        {
            if (anyKeyAction.triggered)
            {
                Debug.Log("Any Key");
                mainMenuAnimation.Play();
                yield break; // keluar
            }

            yield return null; // tunggu frame
        }
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
