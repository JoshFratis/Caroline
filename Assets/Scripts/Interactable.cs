using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public float interactionDistance;
    public string interactionMessage;
    public bool interactionEnabled;
    [SerializeField] UnityEvent onMouseClick;
    
    public void OnMouseClick()
    {
        Debug.Log("click!");
        onMouseClick?.Invoke();
    }
}
