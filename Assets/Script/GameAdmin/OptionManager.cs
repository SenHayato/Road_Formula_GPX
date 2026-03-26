using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public static OptionManager Instance { get; private set; }

    [Header("Volume Slider")]
    [SerializeField] Slider bgmVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;

    [Header("Audio Mixer")]
    [SerializeField] AudioMixer bgmMixer;
    [SerializeField] AudioMixer sfxMixer;

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

    public void EnableOption()
    {
        gameObject.SetActive (true);
    }

    public void DisableOption()
    {
        gameObject.SetActive (false);
    }

    void VolumeManager()
    {
        bgmMixer.SetFloat("BGMVolume", Mathf.Log10(bgmVolumeSlider.value) * 20);
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolumeSlider.value) * 20);
    }

    // Update is called once per frame
    void Update()
    {
        VolumeManager();
    }
}
