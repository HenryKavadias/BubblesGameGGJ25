using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Data Config", menuName = "ScriptableObject/Sound Data Config")]
public class SOSoundData : ScriptableObject
{
    [SerializeField] protected string title = string.Empty;
    [SerializeField, Range(0f, 1f)] protected float volumeScalar = 1f;
    [SerializeField] protected bool randomizePitch = false;
    [SerializeField, Range(-3f, 3f)] protected float pitch = 1f;
    [SerializeField, Range(0f, 4f)] protected float pitchRange = 0f;
    [SerializeField] protected AudioClip audioFile;

    private bool AudioFileNull()
    { return audioFile == null; }
    public AudioClip GetAudioClip()
    { 
        if (AudioFileNull()) { return null; }
        return audioFile; 
    }
    public float GetVolume(float worldVolume) 
    { return volumeScalar * worldVolume; }
    public float GetPitch() 
    { 
        if (!randomizePitch) { return pitch; }
        return Random.Range(
            pitch - pitchRange / 2, 
            pitch + pitchRange / 2); 
    }
    public string GetName()
    {
        if (title == string.Empty) 
        { return name; }
        return title; 
    }
}
