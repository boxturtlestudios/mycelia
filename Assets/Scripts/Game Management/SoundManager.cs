using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private void Awake() 
    {
        #region singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogAssertion("Duplicate SoundManager destroyed!");
            Destroy(gameObject);
        }
        #endregion

        InitializeAudioSources();
    }

    private void InitializeAudioSources()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.mixer;
        }
    }

    public Sound[] sounds;


    //play sound
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: \"" + name + "\" was not found!");
            return;
        }

        s.source.Play();
    }

    //play sound with delay
    public void Play(string name, float fadeRate)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: \"" + name + "\" was not found!");
            return;
        }

        s.source.Play();
        s.source.volume = 0;
        StartCoroutine(FadeIn(s.source, fadeRate));
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: \"" + name + "\" was not found!");
            return;
        }

        s.source.Stop();
    }

    public void Stop(string name, float fadeRate)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: \"" + name + "\" was not found!");
            return;
        }

        StartCoroutine(FadeOut(s.source, fadeRate));
    }

    private IEnumerator FadeIn(AudioSource source, float fadeRate)
    {
        while(source.volume < 1)
        {
            source.volume += fadeRate;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator FadeOut(AudioSource source, float fadeRate)
    {
        while(source.volume > 0)
        {
            source.volume -= fadeRate;
            yield return new WaitForSeconds(0.01f);
        }
        source.Stop();
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;
    public AudioMixerGroup mixer;
    [Range(0f, 1f)] public float volume = 1;
    [Range(0.1f, 3f)] public float pitch = 1;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
