using System;
using System.Collections.Generic;
using UnityEngine;


//[Serializable]
//public class Sound
//{
//    //public SoundType soundType;
//    public string name;
//    public AudioClip clip;
//}

// Holds and managers all the sounds and music tracks in the game
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<SOSoundData> sfxClips;
    public List<SOSoundData> musicTracks;
    public List<SOSoundData> ambienceTracks;
    
    private bool loopMusic = true;
    private bool loopAmbience = true;

    // what each audio setting is
    public float masterVolume { get; set; } = 1.0f;
    public float musicVolume { get; set; } = 1.0f;
    public float sfxVolume { get; set; } = 1.0f;
    public float ambienceVolume { get; set; } = 1.0f;

    // audio volume modified by the master
    public float actualMusicVolume { get; set; } = 1.0f;
    public float actualSFXVolume { get; set; } = 1.0f;
    public float actualAmbienceVolume { get; set; } = 1.0f;

    public AudioSource musicAudioSource;
    public AudioSource ambienceAudioSource;

    public void UpdateMasterVolume(float volume)
    {
        masterVolume = volume;

        UpdateAmbienceVolume(ambienceVolume);
        UpdateMusicVolume(musicVolume);
        UpdateSoundVolume(sfxVolume);
    }
    
    // Updates the volume for the music audio source
    public void UpdateAmbienceVolume(float volume)
    {
        actualAmbienceVolume = volume * masterVolume;
        ambienceVolume = volume;
        ambienceAudioSource.volume = actualAmbienceVolume;
    }

    // Updates the volume for the music audio source
    public void UpdateMusicVolume(float volume)
    {
        actualMusicVolume = volume * masterVolume;      
        musicVolume = volume;
        musicAudioSource.volume = actualMusicVolume;
    }

    public void UpdateSoundVolume(float volume)
    {
        actualSFXVolume = volume * masterVolume;       
        sfxVolume = volume;
    }

    private void Awake()
    {
        if (instance == null)
        { instance = this; }
        else
        {
            Debug.Log("[Singleton] Trying to instantiate a second " +
                "instance of a singleton class (AudioManager).");
            Destroy(gameObject); // removes the duplicate 
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (musicAudioSource == null || ambienceAudioSource == null)
        { Debug.LogWarning("missing audio sources"); }
    }

    public List<SOSoundData> GetSounds()
    {
        return sfxClips;
    }

    private float startAmbienceTime = 0f;
    private float stopAmbienceTime = 0f;
    public string currentAmbience { get; private set; } = string.Empty;

    private float startMusicTime = 0f;
    private float stopMusicTime = 0f;
    public string currentMusicTrack { get; private set; } = string.Empty;

    // Starts a music track at a specific start point with a delay and
    // end point in the track. Also controls if the track loops
    public void BeginMusicTrack(
        string name,
        bool loop = true,
        float start = 0.0f, 
        float stop = 0.0f, 
        float delay = 0.0f)
    {
        musicAudioSource.Stop();

        // check if music is accessable
        bool pass = false;
        int count = 0;

        foreach (SOSoundData music in musicTracks)
        {
            if (music.GetName() == name)
            {
                pass = true;
                break;
            }
            count++;
        }

        if (!pass)
        { return; }

        currentMusicTrack = name;
        stopMusicTime = stop;

        AudioClip musicClip = musicTracks[count].GetAudioClip();
        musicAudioSource.volume = 
            musicTracks[count].GetVolume(actualMusicVolume);
        musicAudioSource.pitch = musicTracks[count].GetPitch();

        musicAudioSource.clip = musicClip;

        if (start > 0f && delay >= 0f)
        {
            musicAudioSource.time = start - delay;
        }
        startMusicTime = start;

        loopMusic = loop;

        musicAudioSource.Play();
        musicAudioSource.loop = loop;
    }

    public void BeginAmbience(
        string name,
        bool loop = true,
        float start = 0.0f,
        float stop = 0.0f,
        float delay = 0.0f)
    {
        ambienceAudioSource.Stop();

        // check if ambience is accessable
        bool pass = false;
        int count = 0;

        foreach (SOSoundData ambience in ambienceTracks)
        {
            if (ambience.GetName() == name)
            {
                pass = true;
                break;
            }
            count++;
        }

        if (!pass)
        { return; }

        currentAmbience = name;
        stopAmbienceTime = stop;

        AudioClip ambienceClip = ambienceTracks[count].GetAudioClip();

        ambienceAudioSource.volume = 
            ambienceTracks[count].GetVolume(actualAmbienceVolume);
        ambienceAudioSource.pitch = ambienceTracks[count].GetPitch();

        ambienceAudioSource.clip = ambienceClip;

        if (start > 0f && delay >= 0f)
        { ambienceAudioSource.time = start - delay; }

        startAmbienceTime = start;

        loopAmbience = loop;

        ambienceAudioSource.Play();
        ambienceAudioSource.loop = loop;
    }

    public void EndMusicTrack()
    { musicAudioSource.Stop(); }

    public void EndAmbience()
    { ambienceAudioSource.Stop(); }

    private void Update()
    {
        if (stopMusicTime > 0f && 
            startMusicTime > 0f && 
            musicAudioSource.time > stopMusicTime)
        {
            musicAudioSource.Stop();

            if (loopMusic)
            {
                musicAudioSource.time = startMusicTime;
                musicAudioSource.Play();
            }
        }

        if (stopAmbienceTime > 0f && 
            startAmbienceTime > 0f && 
            ambienceAudioSource.time > stopAmbienceTime)
        {
            ambienceAudioSource.Stop();

            if (loopAmbience)
            {
                ambienceAudioSource.time = startAmbienceTime;
                ambienceAudioSource.Play();
            }
        }
    }
}
