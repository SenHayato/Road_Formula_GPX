using UnityEngine;
using UnityEngine.UI;

public class OptionScript : MonoBehaviour
{
    public static OptionScript instance;

    [Header("Volume Setting")]
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    [Header("Option Window")]
    [SerializeField] GameObject optionScreen;

    //[Header("Resolution Setting")]

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
        optionScreen.SetActive(false);
    }

    #region OptionButtonConfig

    public void BackButton()
    {
        optionScreen.SetActive(false);
    }

    #endregion

    void Update()
    {
        DontDestroyOnLoad(gameObject); //supaya tidak hilang ketika load scene game
    }
}
