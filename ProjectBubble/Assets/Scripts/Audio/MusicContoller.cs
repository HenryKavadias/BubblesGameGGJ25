using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the music for the scene
public class MusicContoller : MonoBehaviour
{
    private GameObject objectMusic = null;

    public bool playOnStart = true;
    public bool modifyTrack = false;
    public string track = string.Empty;
    public float startOfTrack = 0f;
    public float endOfTrack = 0f;
    public float initialDelay = 0f;
    public bool loopTrack = true;

    [SerializeField, Range(0f, 1f)] private float defaultMusicVolume = 0.5f;   // default volume
    private AudioSource audioSource;
    private AudioManager audioManager;

    // Gets music source, gets the audio source from it, and sets its volume
    void Start()
    {
        objectMusic = GameObject.FindWithTag("GameAudio");

        if (objectMusic)
        {
            audioManager = objectMusic.GetComponent<AudioManager>();

            audioSource = audioManager.musicAudioSource;

            if (audioManager != null ) 
            {
                audioSource.volume = audioManager.actualMusicVolume;

                if (track != string.Empty && track != audioManager.currentMusicTrack)
                {
                    if (!playOnStart) { return; }
                    StartTrack();
                }
            }
            else
            {
                audioSource.volume = defaultMusicVolume;
            }
        }
    }

    public void StartTrack()
    {
        if (modifyTrack)
        {
            audioManager.BeginMusicTrack(
                        track, loopTrack, startOfTrack,
                        endOfTrack, initialDelay);

            return;
        }
        audioManager.BeginMusicTrack(
                        track, loopTrack);
    }

    // Stops the current music track
    public void StopTrack()
    {
        if (audioManager != null)
        {
            audioManager.EndMusicTrack();
        }
    }

    // Below is used for pause controls

    // Tracks pause state of music
    public bool isPaused { get; private set; }

    // Toggles pause state of music
    public void PauseMusic(bool pause = true)
    {
        if (!audioSource) { return; }

        if (pause && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
        isPaused = pause;
    }
}
