using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        public bool loop;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
    }

    public Sound[] sounds;

    private Dictionary<string, AudioSource> soundDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
          //  DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        soundDictionary = new Dictionary<string, AudioSource>();
        foreach (Sound s in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.outputAudioMixerGroup = s.mixerGroup;
            source.clip = s.clip;
            source.loop = s.loop;
            source.volume = s.volume;
            source.pitch = s.pitch;
            soundDictionary[s.name] = source;
        }
    }
    public void Play(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            soundDictionary[soundName].Play();
        }
        else
        {
            Debug.LogWarning("AudioManager: Sound not found: " + soundName);
        }
    }

    public void Stop(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            soundDictionary[soundName].Stop();
        }
        else
        {
            Debug.LogWarning("AudioManager: Sound not found: " + soundName);
        }
    }
}
