using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRocks : MonoBehaviour
{ 
    Rigidbody rigidbody;
    Rigidbody[] fallingRockChildren;

    void Start()
    {
        fallingRockChildren = GetComponentsInChildren<Rigidbody>();
    }

    public void Fall()
    {
        // Iterate through rocks, enable physics
        for(int i = 0; i < fallingRockChildren.Length; i++)
        {
            rigidbody = fallingRockChildren[i];
            rigidbody.isKinematic = false;
            Debug.Log("Rigidbody Kinetmatic True");
        }
    }
}
