using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] float initIntensity;
    [SerializeField] float intensityMin;
    [SerializeField] float rateOfDecay;
    [SerializeField] float decayOnSwing;
    [SerializeField] AudioSource audioSourceSwing;
    [SerializeField] AudioSource audioSourceFlame;
    private float intensityMax;
    public float intensity;
    public bool isSwinging;
    
    //can i serialize these please?
    LightCollider lightColliderScript;
    Transform thisTransform;
    Transform lightSourceTransform;
    Transform lightColliderTransform;
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        intensity = initIntensity;
        intensityMax = initIntensity;
        lightColliderScript = GetComponentInChildren<LightCollider>();
        thisTransform = GetComponent<Transform>();
        lightSourceTransform = thisTransform.GetChild(0);
        lightSourceTransform = lightSourceTransform.GetChild(0);
        lightColliderTransform = lightSourceTransform.GetChild(1);
        audioSourceSwing.Play(0);
    }

    void Update()
    {
        // gradually decay torch intensity
        setIntensity(intensity - rateOfDecay);

        if (audioSourceSwing.isPlaying)
        {
            isSwinging = true;
        }
        else if (!audioSourceSwing.isPlaying)
        {
            isSwinging = false;
        }

        if ((Input.GetMouseButtonDown(0)) && (!isSwinging))
        {
            Swing();
        }
    }

    public void Swing()
    {
        if (!audioSourceSwing.isPlaying) 
        {
            audioSourceSwing.Play(0);
            setIntensity(intensity - decayOnSwing);
        }
    }

    private void setIntensity(float newIntensity)
    {
        bool showNewIntensity = false;
        if (((newIntensity - intensity) > 1) || (intensity % 25 == 0))
        {
            showNewIntensity = true;
        }

        // Set New Intensity
        intensity = Mathf.Clamp(newIntensity, intensityMin, intensityMax);

        // Set light range accordingly
        lightColliderScript.setRadius(intensity);

        // Scale Fire Transform (and by extention its Collider child component's Radius)
        float newIntensityCent = intensity / initIntensity;
        Vector3 newIntensityVector = new Vector3(newIntensityCent, newIntensityCent, newIntensityCent);
        lightSourceTransform.localScale = newIntensityVector;

        // Set volume accordingly
        audioSourceFlame.volume = newIntensityCent;
        audioSourceSwing.volume = newIntensityCent;

        if (showNewIntensity)
        {
            Debug.Log("Torch Intensity: "+intensity);
        }
    }

    // Set light's torch to maximum
    public void Light()
    {
        setIntensity(initIntensity);    // goes first bc volume dependent on intensity
                                        // calling Swing() would require setIntensity() before (bc ^^) and after bc Swing() decrements intensity
        audioSourceSwing.Play(0);
    }
}
