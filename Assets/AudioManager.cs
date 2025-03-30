using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] music, sfx;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Music");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(music, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Music not found");
            return;
        }

        musicSource.clip = s.GetRandomClip();
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfx, x => x.name == name);

        if (s == null)
        {
            Debug.Log("SFX not found: " + name);
            return;
        }

        AudioClip clip = s.GetRandomClip();
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void ToggleMusic() => musicSource.mute = !musicSource.mute;
    public void ToggleSFX() => sfxSource.mute = !sfxSource.mute;
    public void MusicVolume(float volume) => musicSource.volume = volume;
    public void SFXVolume(float volume) => sfxSource.volume = volume;
}

