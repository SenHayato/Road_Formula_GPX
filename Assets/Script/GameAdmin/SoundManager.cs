using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("SFX Library")]
    [SerializeField] AudioClip[] sfxClips;

    [Header("Car Crash Impact Library Sound")]
    [SerializeField] AudioClip[] crashImpactClips;

    [Header("Audio Component")]
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
}
