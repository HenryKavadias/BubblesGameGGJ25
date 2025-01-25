using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.ShaderData;

// Gets the list of sounds from the audio manager based on given string names
public class SFXController : MonoBehaviour
{
    public bool playOnSpawn = false;
    
    [Header("Must be accurate (case sensitive)")]
    public List<string> sfxNames = new List<string>();
    
    [SerializeField] private AudioSource audioSource; 

    private List<SOSoundData> sfxData = new List<SOSoundData>();

    [Range(0f, 1f)] private float volume = 1.0f;

    void Awake()
    {
        List<SOSoundData> soundList = null;
        soundList = GameObject.FindGameObjectWithTag("GameAudio").
            gameObject.GetComponent<AudioManager>().GetSounds();

        if (soundList != null)
        {
            volume = GameObject.FindGameObjectWithTag("GameAudio").
                gameObject.GetComponent<AudioManager>().actualSFXVolume;
            foreach (SOSoundData sound in soundList)
            {
                foreach (string wanted in sfxNames)
                {
                    if (sound.GetName() == wanted)
                    { sfxData.Add(sound); }
                }
            }
        }

        if (!gameObject.GetComponent<AudioSource>())
        { audioSource = gameObject.AddComponent<AudioSource>(); }

        if (!audioSource)
        { audioSource = gameObject.GetComponent<AudioSource>(); }

        if (playOnSpawn)
        { RandomSFX(); }
    }

    protected void OnEnable()
    {
        if (playOnSpawn)
        { RandomSFX(); }
    }

    public void AssignSounds(List<string> sfxs)
    {
        sfxNames = new List<string>(sfxs);

        // Refresh
        Awake();

        if (playOnSpawn)
        { RandomSFX(); }
    }

    // Plays a random sound in the trigger sounds list 
    public void RandomSFX()
    {
        // Randomly pick a sound from the current list
        int randomIndex = Random.Range(0, sfxData.Count);
        string soundName = sfxNames[randomIndex];

        PlayAudio(soundName);
    }

    // Plays a random sound from the given list of sound names (if the script has access to them)
    public void PlayRandomAudioFromList(List<string> sounds)
    {
        int randomIndex = Random.Range(0, sounds.Count);
        string soundName = sounds[randomIndex];
        
        PlayAudio(soundName);
    }

    // Trigger a specific sound in the sounds list
    public void PlayAudio(string soundName)
    {
        // check if sound is accessable to object
        bool pass = false;
        int count = 0;

        foreach (string sound in sfxNames)
        {
            if (sound == soundName)
            {
                pass = true;
                break;
            }
            count++;
        }

        if (!pass)
        {
            Debug.LogWarning("Sound doesn't Exist");
            return; 
        }

        AudioClip soundClip = sfxData[count].GetAudioClip();

        float pitch = sfxData[count].GetPitch();
        float vol = sfxData[count].GetVolume(volume);

        // Play audio one shot
        audioSource.Stop();
        audioSource.volume = vol;
        audioSource.pitch = pitch;
        audioSource.Play();

        audioSource.clip = soundClip;

        if (audioSource.isPlaying) { return; }

        audioSource.Play();

        //audioSource.PlayOneShot(soundClip);
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
