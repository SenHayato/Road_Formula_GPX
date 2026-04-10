using System.Collections;
using TMPro;
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

    [Header("UI Pop UP Music")]
    [SerializeField] GameObject popUpMusic;
    [SerializeField] TextMeshProUGUI musicName;
    [SerializeField] float musicPopUpDuration;

    [Header("Component")]
    [SerializeField] AudioSource musicSource;

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
    }

    private void Start()
    {
        bgmNumber = Mathf.Clamp(bgmNumber, 1, bgmClips.Length);
        StartCoroutine(PlayMusic());
    }

    int clipNumber;
    bool lastRandomState;
    IEnumerator PlayMusic()
    {
        lastRandomState = isBGMRandom;

        while (true)
        {
            if (lastRandomState != isBGMRandom)
            {
                musicSource.Stop();
                lastRandomState = isBGMRandom;
            }

            if (isBGMRandom)
            {
                clipNumber = Random.Range(0, bgmClips.Length);
                bgmNumber = clipNumber + 1;

                musicSource.loop = false;
                musicSource.clip = bgmClips[clipNumber];
                musicSource.Play();

                yield return new WaitWhile(() => musicSource.isPlaying);
            }
            else
            {
                clipNumber = Mathf.Clamp(bgmNumber - 1, 0, bgmClips.Length - 1);

                if (musicSource.clip != bgmClips[clipNumber])
                {
                    musicSource.loop = true;
                    musicSource.clip = bgmClips[clipNumber];
                    musicSource.Play();
                }

                yield return null;
            }
        }
    }
}
