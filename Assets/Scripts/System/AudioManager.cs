using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public float volume = 1f;
    }

    public Sound[] sounds;
    private Dictionary<string, Sound> soundDict = new Dictionary<string, Sound>();
    private AudioSource audioSource;

    void Awake()
    {
        Instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();

        foreach (var sound in sounds)
            soundDict[sound.name] = sound;
    }

    public void PlaySFX(string soundName, float? customVolume = null)
    {
        if (soundDict.TryGetValue(soundName, out Sound sound))
        {
            float volumeToUse = customVolume ?? sound.volume;
            audioSource.PlayOneShot(sound.clip, volumeToUse);
        }
    }
}
