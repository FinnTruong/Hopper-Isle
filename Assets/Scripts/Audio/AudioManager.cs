﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    AudioSource[] musicSources;
    public AudioSource[] ambienceSources;
    AudioSource sfxSources;
    AudioSource collisionSFXSource;

    int activeMusicSourceIndex;
    int activeAmbienceSourceIndex;


    SoundLibrary library;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        library = GetComponent<SoundLibrary>();

        SetupMusicSources();
        SetupAmbienceSources();
        SetupSFXSources();
        SetupCollisionSFXSource();

    }

    void SetupMusicSources()
    {
        musicSources = new AudioSource[2];
        for (int i = 0; i < 2; i++)
        {
            GameObject newMusicSources = new GameObject("Music Sources " + (i + 1));
            musicSources[i] = newMusicSources.AddComponent<AudioSource>();
            musicSources[i].loop = true;
            musicSources[i].volume = 0.7f;
            newMusicSources.transform.parent = transform;
        }
    }

    void SetupAmbienceSources()
    {
        ambienceSources = new AudioSource[2];
        for (int i = 0; i < 2; i++)
        {
            GameObject newMusicSources = new GameObject("Ambience Source " + (i + 1));
            ambienceSources[i] = newMusicSources.AddComponent<AudioSource>();
            ambienceSources[i].loop = true;
            newMusicSources.transform.parent = transform;
        }
    }

    void SetupSFXSources()
    {
        GameObject newSFXSource = new GameObject("SFX Source");
        sfxSources = newSFXSource.AddComponent<AudioSource>();
        newSFXSource.transform.parent = transform;
    }

    void SetupCollisionSFXSource()
    {
        GameObject newCollisionSFXSource = new GameObject("Collision SFX Source");
        collisionSFXSource = newCollisionSFXSource.AddComponent<AudioSource>();
        newCollisionSFXSource.transform.parent = transform;
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex; //The active music source will switch between 0 and 1

        musicSources[activeMusicSourceIndex].clip = clip;
        if (clip != null)
            musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossFade(fadeDuration));
    }

    public void PlayMusic(AudioClip clip)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex; //The active music source will switch between 0 and 1

        musicSources[activeMusicSourceIndex].clip = clip;
        if (clip != null)
            musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossFade(1f));
    }

    //public void StopMusic(float fadeDuration = 1)
    //{
    //    StartCoroutine(FadeOutMusic(fadeDuration));
    //}

    public void PlayAmbience(AudioClip clip, float fadeDuration = 0.5f)
    {
        activeAmbienceSourceIndex = 1 - activeAmbienceSourceIndex; //The active music source will switch between 0 and 1

        ambienceSources[activeAmbienceSourceIndex].clip = clip;
        if (clip != null)
            ambienceSources[activeAmbienceSourceIndex].Play();

        StartCoroutine(AnimateAmbienceCrossfade(fadeDuration));
    }

    public void PlayAmbience(AudioClip clip)
    {
        activeAmbienceSourceIndex = 1 - activeAmbienceSourceIndex; //The active music source will switch between 0 and 1

        ambienceSources[activeAmbienceSourceIndex].clip = clip;
        if (clip != null)
            ambienceSources[activeAmbienceSourceIndex].Play();

        StartCoroutine(AnimateAmbienceCrossfade(0.5f));
    }




    public void PlaySFX(string name, float volume = 0.7f)
    {
        sfxSources.PlayOneShot(library.GetClipFromName(name), volume);
    }

    public void PlaySFX(AudioClip clip, float volume = 0.7f)
    {
        sfxSources.PlayOneShot(clip, volume);
    }

    public void StopSFX()
    {
        sfxSources.Stop();
    }

    public void PlayCollisionSFX(string name, float volume)
    {
        collisionSFXSource.clip = library.GetClipFromName(name);
        collisionSFXSource.volume = volume;
        collisionSFXSource.Play();
    }

    public void StopCollisionSFX()
    {
        collisionSFXSource.Stop();
    }


    //IEnumerator FadeInMusic(float duration)
    //{
    //    float percent = 0;

    //    while (percent < 1)
    //    {
    //        percent += Time.deltaTime * 1 / duration;
    //        musicSources.volume = Mathf.Lerp(0, 1, percent);
    //        yield return null;
    //    }
    //}

    //IEnumerator FadeOutMusic(float duration)
    //{
    //    float percent = 0;

    //    while (percent < 1)
    //    {
    //        percent += Time.deltaTime * 1 / duration;
    //        musicSources.volume = Mathf.Lerp(1, 0, percent);
    //        yield return null;
    //    }
    //}

    IEnumerator AnimateMusicCrossFade(float duration = 1f)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, 0.7f, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(0.7f, 0, percent);
            yield return null;
        }
    }

    IEnumerator AnimateAmbienceCrossfade(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            ambienceSources[activeAmbienceSourceIndex].volume = Mathf.Lerp(0, 1, percent);
            ambienceSources[1 - activeAmbienceSourceIndex].volume = Mathf.Lerp(1, 0, percent);
            yield return null;
        }
    }
}
