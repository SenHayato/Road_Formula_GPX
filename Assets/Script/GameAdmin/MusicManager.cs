using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Game Music List")]
    [SerializeField] AudioClip[] musicClip;

    [Header("Audio Component")]
    [SerializeField] bool isPlaying = false;
    [SerializeField] AudioSource musicSource;

    void Awake()
    {
        musicSource = GetComponent<AudioSource>();
    }

    void Start()
    {

    }

    void MusicPlayList()
    {

    }

    void Update()
    {
        
    }
}
