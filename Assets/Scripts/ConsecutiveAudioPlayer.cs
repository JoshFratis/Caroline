using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsecutiveAudioPlayer : MonoBehaviour
{
    [SerializeField] GameObject randomAudioSource;

    [SerializeField] int audioTimerMin = 100;
    [SerializeField] int audioTimerMax = 1000;
    [SerializeField] float audioVolumeMin = 0.1f;
    [SerializeField] float audioVolumeMax = 0.25f;

    AudioSource[] randomAudio;
    AudioSource audioPlaying;
    int randomAudioTimer;
    bool playAudio = false;

    void Start()
    {
        randomAudio = randomAudioSource.GetComponentsInChildren<AudioSource>();
    }

    void Update()
    {
        if (playAudio == true)
        {
            if (randomAudioTimer <= 0)
            {
                if (!audioPlaying.isPlaying)
                {
                    Activate();
                    audioPlaying.Play(0);
                }
                else
                {
                    randomAudioTimer = Random.Range(audioTimerMin, audioTimerMax);
                }
            }
            else
            {
                randomAudioTimer--;
            }
        }
    }

    public void Activate()
    {
        Debug.Log("baby activate!");
        playAudio = true;  
        audioPlaying = randomAudio[Random.Range(0, randomAudio.Length)];
        audioPlaying.volume = Random.Range(audioVolumeMin, audioVolumeMax);
        randomAudioTimer = Random.Range(audioTimerMin, audioTimerMax);
    }

    public void Deactivate()
    {
        randomAudioTimer = 0;
        playAudio = false;
        Debug.Log("baby DEactivate!");
    }

    public void Stop()
    {
        randomAudioTimer = 0;
        playAudio = false;randomAudioTimer = 0;
        audioPlaying.Stop();
        Debug.Log("baby stop!");
    }
}
