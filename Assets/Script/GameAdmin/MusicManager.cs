using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField] RectTransform popUpMusic;
    [SerializeField] TextMeshProUGUI musicName;
    [SerializeField] float musicPopUpDuration;
    [SerializeField] Ease animEase;

    [Header("Component")]
    [SerializeField] AudioSource musicSource;
    bool gameIsOver = false;

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
        Invoke(nameof(PlayBGMSystem), 8f);
    }

    public void PlayBGMSystem()
    {
        StartCoroutine(PlayMusic());
    }

    int clipNumber;
    bool lastRandomState;
    bool isPopUp = false;
    IEnumerator PlayMusic()
    {
        lastRandomState = isBGMRandom;

        while (true)
        {
            if (gameIsOver)
            {
                musicSource.Stop();
                yield break;
            }

            if (bgmClips.Length == 0)
            {
                yield return null;
                continue;
            }

            if (lastRandomState != isBGMRandom)
            {
                musicSource.Stop();
                lastRandomState = isBGMRandom;
                yield return null;
                continue;
            }

            if (!isPopUp)
            {
                MusicPopUpToggle();
                isPopUp = true;
            }

            if (isBGMRandom)
            {
                int newClip;
                do
                {
                    newClip = Random.Range(0, bgmClips.Length);
                } while (newClip == clipNumber && bgmClips.Length > 1);

                clipNumber = newClip;
                bgmNumber = clipNumber + 1;

                musicName.text = bgmClips[clipNumber].name;

                musicSource.loop = false;
                musicSource.clip = bgmClips[clipNumber];
                musicSource.Play();

                while (musicSource.isPlaying)
                    yield return null;
            }
            else
            {
                clipNumber = Mathf.Clamp(bgmNumber - 1, 0, bgmClips.Length - 1);

                if (musicSource.clip != bgmClips[clipNumber] || !musicSource.isPlaying)
                {
                    musicSource.Stop();

                    musicName.text = bgmClips[clipNumber].name;
                    musicSource.loop = true;
                    musicSource.clip = bgmClips[clipNumber];
                    musicSource.Play();
                }

                yield return null;
            }
        }
    }

    void MusicPopUpToggle()
    {
        popUpMusic.DOAnchorPosX(0, musicPopUpDuration).SetEase(animEase);
        Invoke(nameof(MusicPopUpReset), 4f);
        //Debug.Log("Pop Up Music Enable");
    }

    void MusicPopUpReset()
    {
        isPopUp = false;
        popUpMusic.DOAnchorPosX(-446, musicPopUpDuration).SetEase(animEase);
        //Debug.Log("Pop Up Music Disable");
    }

    public void StopGameMusic()
    {
        gameIsOver = true;
        StopCoroutine(PlayMusic());
        musicSource.Stop();
        //Debug.Log("Musik Berhenti Kalah");
    }
}
