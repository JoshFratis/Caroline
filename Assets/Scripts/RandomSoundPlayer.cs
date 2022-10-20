using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField] GameObject randomAudioSource;

    [SerializeField] int audioTimerMin = 100;
    [SerializeField] int audioTimerMax = 1000;
    [SerializeField] float audioVolumeMin = 0.1f;
    [SerializeField] float audioVolumeMax = 0.25f;

    AudioSource[] randomAudio;
    int[] randomAudioTimer;
    bool isPlaying = false;

    void Start()
    {
        Debug.Log("Audio Starting");
        // Random Audio (while despawned)
        randomAudio = randomAudioSource.GetComponentsInChildren<AudioSource>();
        randomAudioTimer = new int[randomAudio.Length]; 
        for (int i = 0; i < randomAudioTimer.Length; i++) // this necessary?
        {
            randomAudioTimer[i] = Random.Range(audioTimerMin, audioTimerMax);
            randomAudio[i].volume = Random.Range(audioVolumeMin, audioVolumeMax);
        }
    }

    void Update()
    {
        if (isPlaying = true)
        {
            PlayRandomAudio();
        }
    }

    public void Activate()
    {
        for(int i = 0; i < randomAudio.Length; i++)
        {
            randomAudioTimer[i] = Random.Range(audioTimerMin, audioTimerMax);
        }
        isPlaying = true;
        Debug.Log("isPlaying = true");
    }

    public void Deactivate()
    {
        isPlaying = false;
        Debug.Log("isPlaying = false");
        for(int i = 0; i < randomAudio.Length; i++)
        {
            randomAudioTimer[i] = 0;
        }
    }

    public void Stop()
    {
        isPlaying = false;
        Debug.Log("isPlaying = false");
        for(int i = 0; i < randomAudio.Length; i++)
        {
            randomAudioTimer[i] = 0;
            randomAudio[i].Stop();
        }
    }

    // Play Random Audio
    public void PlayRandomAudio()
    {
        Debug.Log("Playing random audio");
        for(int i = 0; i < randomAudio.Length; i++)
        {
            if (randomAudioTimer[i] <= 0) 
            {
                if (!randomAudio[i].isPlaying)
                {
                    randomAudio[i].Play(0);
                    Debug.Log("Playing audio #"+i);
                }
                else
                {
                    // Don't restart sound mid-play
                    randomAudioTimer[i] = Random.Range(audioTimerMin, audioTimerMax);
                }
                
                // Randomize Volume
                randomAudio[i].volume = Random.Range(audioVolumeMin, audioVolumeMax);
            }
            else 
            {
                randomAudioTimer[i]--;
            }
        }
    }
}
