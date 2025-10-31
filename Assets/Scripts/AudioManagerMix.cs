using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerMix : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name; // Уникальное имя для идентификации звука
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
        public bool loop = false;

        [HideInInspector]
        public AudioSource source;
    }

    public List<Sound> sounds = new List<Sound>();

    void Awake()
    {
        // Создаем AudioSource для каждого звука в списке
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // Метод для воспроизведения звука по имени
    public void Play(string soundName)
    {
        Sound s = sounds.Find(sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        s.source.Play();
    }

    // Дополнительные методы: Stop, Pause, SetVolume и т.д.
    public void Stop(string soundName)
    {
        Sound s = sounds.Find(sound => sound.name == soundName);
        if (s == null) return;
        s.source.Stop();
    }

    // Метод для воспроизведения звука без привязки к конкретному AudioSource (удобно для UI)
    public void PlayOneShot(string soundName)
    {
        Sound s = sounds.Find(sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        s.source.PlayOneShot(s.clip);
    }
}
