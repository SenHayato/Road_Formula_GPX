using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("BGM List")]
    [SerializeField] int bgmNumber;  //nilai bisa di kurangi satu untuk menyesuaikan dengan Array list BGM
    [SerializeField] AudioClip[] bgmClips;

    [Header("BGM Condition")]
    [SerializeField] bool isBGMPlaying = false;
    [SerializeField] bool isBGMRandom = true;

    [Header("SFX Library")]
    [SerializeField] AudioClip[] sfxClips;

    [Header("Car Crash Impact Library Sound")]
    [SerializeField] AudioClip[] crashImpactClips;

    [Header("Audio Component")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

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

    public void PlaySFXOnce(AudioSource audioSourceObj, string clipName)
    {
        if (audioSourceObj == null)
        {
            audioSourceObj = sfxSource;
        }

        foreach (var clip in sfxClips)
        {
            if (clip.name == clipName)
            {
                audioSourceObj.PlayOneShot(clip);
                return;
            }
        }
    }

    public void PlayCrashSFX(AudioSource audioSourceObj)
    {
        if (audioSourceObj == null)
        {
            audioSourceObj = sfxSource;
        }

        int sfxNum = Random.Range(0, crashImpactClips.Length);
        audioSourceObj.PlayOneShot(crashImpactClips[sfxNum]);
        return;
    }


    #region Test UniTask
    public int second;
    public async UniTask TestUni()
    {
        transform.position = 8f * Time.deltaTime * Vector3.up;
        await UniTask.Delay(second * 1000); //pengganti yield return new WaitforSeconds
        transform.position = Vector3.zero;

        return; //pengganti yield break;
    }
    #endregion

    void Update()
    {
        TestUni().Forget();
    }
}
