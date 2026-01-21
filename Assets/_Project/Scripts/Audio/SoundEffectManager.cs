using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance { get; private set; }

    [SerializeField] private Slider _sfxSlider;

    private static AudioSource _audioSource;
    private static AudioSource _randomPitchAudioSource;
    private static AudioSource _voiceAudioSource;
    private static SoundEffectLibrary _soundEffectLibrary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        AudioSource[] audioSourceArray = GetComponents<AudioSource>();
        _audioSource = audioSourceArray[0];
        _randomPitchAudioSource = audioSourceArray[1];
        _voiceAudioSource = audioSourceArray[2];
        _soundEffectLibrary = GetComponent<SoundEffectLibrary>();
    }

    private void Start()
    {
        _sfxSlider.onValueChanged.AddListener(delegate
        {
            OnValueChanged();
        });

        SetVolume(_sfxSlider.value = 0.1f);
    }

    public static void Play(string soundName, bool randomPitch = false)
    {
        AudioClip audioClip = _soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip == null) return;

        if (randomPitch)
        {
            _randomPitchAudioSource.pitch = Random.Range(1f, 1.5f);
            _randomPitchAudioSource.PlayOneShot(audioClip);
        }
        else
            _audioSource.PlayOneShot(audioClip);
    }

    public static void PlayVoice(AudioClip audioClip, float pitch = 1f)
    {
        _voiceAudioSource.pitch = pitch;
        _voiceAudioSource.PlayOneShot(audioClip);
    }

    public static void SetVolume(float volume)
    {
        _audioSource.volume = volume;
        _randomPitchAudioSource.volume = volume;
        _voiceAudioSource.volume = volume;
    }

    public void OnValueChanged()
    {
        SetVolume(_sfxSlider.value);
    }
}