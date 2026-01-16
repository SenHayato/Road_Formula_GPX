using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Game Music List")]
    [SerializeField] AudioClip[] musicClip;

    [Header("Audio Component")]
    [SerializeField] bool isPlaying = false;
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
