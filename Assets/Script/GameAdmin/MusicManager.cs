using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("BGM List")]
    [SerializeField] int bgmNumber;  //nilai bisa di kurangi satu untuk menyesuaikan dengan Array list BGM
    [SerializeField] AudioClip[] bgmClips;

    [Header("BGM Condition")]
    [SerializeField] bool isBGMPlaying = false;
    [SerializeField] bool isBGMRandom = true;

    [Header("SFX Library")]
    [SerializeField] AudioClip[] sfxClips;

    [Header("Audio Component")]
    [SerializeField] AudioSource musicSource;


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

        //musicSource = GetComponent<AudioSource>();
    }

    void Start()
    {

    }

    public void MusicPlayList()
    {
        //Debug.Log("Test Music");
    }

    void Update()
    {
        
    }
}
