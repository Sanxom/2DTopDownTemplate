using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] private SoundEffectGroup[] _soundEffectGroupArray;

    private Dictionary<string, List<AudioClip>> _soundDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    [System.Serializable]
    public struct SoundEffectGroup
    {
        public string soundName;
        public List<AudioClip> audioClips;
    }

    public AudioClip GetRandomClip(string soundName)
    {
        if (!_soundDictionary.ContainsKey(soundName))
            return null;

        List<AudioClip> audioClips = _soundDictionary[soundName];
        if (audioClips.Count > 0)
            return audioClips[Random.Range(0, audioClips.Count)];

        return null;
    }

    private void InitializeDictionary()
    {
        _soundDictionary = new();

        foreach (SoundEffectGroup soundEffectGroup in _soundEffectGroupArray)
            _soundDictionary[soundEffectGroup.soundName] = soundEffectGroup.audioClips;
    }
}