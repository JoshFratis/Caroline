using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCollider : MonoBehaviour
{

    public float radius;
    Light light;
    SphereCollider sphereCollider;
    
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        light = GetComponent<Light>();
    }

    // Update collider and light components based on radius variable
    void Update()
    {
        sphereCollider.radius = radius;
        light.range = radius / 2;
    }

    public void setRadius(float newRadius)
    {
        radius = newRadius;
    }
}
