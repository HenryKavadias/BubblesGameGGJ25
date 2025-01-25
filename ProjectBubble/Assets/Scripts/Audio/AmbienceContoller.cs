using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the ambience for the scene
public class AmbienceController : MonoBehaviour
{
    private GameObject objectAmbience = null;

    public string track = string.Empty;
    public float startOfTrack = 0f;
    public float endOfTrack = 0f;
    public float initialDelay = 0f;
    public bool loopTrack = true;

    [SerializeField, Range(0f, 1f)] private float defaultAmbienceVolume = 0.5f;   // default volume
    private AudioSource audioSource;
    private AudioManager audioManager;

    // Gets ambience source, gets the audio source from it, and sets its volume
    void Start()
    {
        objectAmbience = GameObject.FindWithTag("GameAudio");

        if (objectAmbience)
        {
            audioManager = objectAmbience.GetComponent<AudioManager>();

            audioSource = audioManager.ambienceAudioSource;

            if (audioManager != null ) 
            {
                audioSource.volume = audioManager.actualAmbienceVolume;

                if (track != string.Empty && track != audioManager.currentAmbience)
                {
                    audioManager.BeginAmbience(
                        track, loopTrack, startOfTrack, endOfTrack, initialDelay);
                }
            }
            else
            {
                audioSource.volume = defaultAmbienceVolume;
            }
        }
    }

    // Stops the current ambience track
    public void StopTrack()
    {
        if (audioManager != null)
        {
            audioManager.EndAmbience();
        }
    }

    // Below is used for pause controls

    // Tracks pause state of ambience
    public bool isPaused { get; private set; }

    // Toggles pause state of ambience
    public void PauseAmbience(bool pause = true)
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
