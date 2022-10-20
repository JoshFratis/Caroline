using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] GameObject randomAudioSource;
    [SerializeField] bool playOnAwake;

    [SerializeField] int soundsPlayingMax = -1;
    [SerializeField] int audioTimerMin = 100;
    [SerializeField] int audioTimerMax = 1000;
    [SerializeField] float audioVolumeMin = 0.1f;
    [SerializeField] float audioVolumeMax = 0.25f;

    AudioSource[] randomAudio;
    int[] randomAudioTimer;
    bool playSound;
    int soundsPlaying = 0;

    void Start()
    {
        Debug.Log("Audio Starting");

        randomAudioSource.SetActive(true);
        randomAudio = randomAudioSource.GetComponentsInChildren<AudioSource>();
        randomAudioTimer = new int[randomAudio.Length]; 

/*
        for(int i = 0; i < randomAudio.Length; i++)
        {
            randomAudio[i].enabled = true;
            randomAudio[i].loop = false;
            randomAudio[i].playOnAwake = false;
        }
        */

        if (soundsPlayingMax < 0)
        {
            soundsPlayingMax = randomAudio.Length;
        }

        if (playOnAwake)
        {
            Activate();
        }

        Debug.Log("soundsPlayingMax: "+soundsPlayingMax);
        Debug.Log("soundsPlaying: "+soundsPlaying);
    }

    void Update()
    {
        if (playSound)
        {
            soundsPlaying = GetSoundsPlaying();
            PlayRandomAudio();
        }
    }

    public int GetSoundsPlaying()
    {
        soundsPlaying = 0;
        for(int i = 0; i < randomAudio.Length; i++)
        {
            if (randomAudio[i].isPlaying)
            {
                soundsPlaying++;
            }
        }
        return soundsPlaying;
    }

    public void SetSoundsPlaying(int newSoundsPlaying)
    {
        Debug.Log("SetSoundsPlaying("+newSoundsPlaying+")");
        soundsPlayingMax = newSoundsPlaying;
        soundsPlaying = 0;
        for(int i = 0; i < randomAudio.Length; i++)
        {
            if (randomAudio[i].isPlaying)
            {
                if (soundsPlaying >= soundsPlayingMax)
                {
                    Deactivate(i);
                }
                else
                {
                    soundsPlaying++;
                }
            }
        }
    }

    public void PlayRandomAudio()
    {
        for(int i = 0; i < randomAudio.Length; i++)
        {
            if (randomAudioTimer[i] <= 0) 
            {
                if ((!randomAudio[i].isPlaying) && (soundsPlaying < soundsPlayingMax))
                {
                    PlaySound(i);
                }
                else
                {
                    randomAudioTimer[i] = Random.Range(audioTimerMin, audioTimerMax);
                    Debug.Log("Reshuffling randomAudioTimer["+i+"] to "+randomAudioTimer[i]);
                }
            }
            else 
            {
                randomAudioTimer[i]--;
            }
        }
    }

    private void PlaySound(int soundIndex)
    {
        Debug.Log("PlaySound("+soundIndex+")");
        if (soundsPlaying < soundsPlayingMax)
        {
            randomAudio[soundIndex].volume = Random.Range(audioVolumeMin, audioVolumeMax);   
            randomAudio[soundIndex].Play(0);
            soundsPlaying++;
            Debug.Log("soundsPlaying: "+soundsPlaying);
        }
        else
        {
            Debug.Log("Error: Unable to play sound. soundsPlaying >= soundsPlayingMax.");
        }
    }

    public void Activate()
    {
        Debug.Log("Activate");
        for(int i = 0; i < randomAudio.Length; i++)
        {
            randomAudioTimer[i] = Random.Range(audioTimerMin, audioTimerMax);
            Debug.Log("Reshuffling randomAudioTimer["+i+"] to "+randomAudioTimer[i]);
        }
        playSound = true;
    }

    public void Deactivate()
    {
        playSound = false;
        Debug.Log("Deactivate");
        for(int i = 0; i < randomAudio.Length; i++)
        {
            randomAudioTimer[i] = 0;
        }
    }

    public void Deactivate(int soundIndex)
    {
        Debug.Log("Deactivate("+soundIndex+")");
        randomAudioTimer[soundIndex] = 0;
    }

    public void Stop()
    {
        Debug.Log("Stop");
        playSound = false;
        for(int i = 0; i < randomAudio.Length; i++)
        {
            randomAudioTimer[i] = 0;
            randomAudio[i].Stop();
        }
    }
}
