using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] float healthMax;
    [SerializeField] float healthCurrent;

    [SerializeField] UnityEvent<float> onHealthPercentChange;

    [SerializeField] UnityEvent onDeath;

    // Called on colliding with damager
    public void Damage(float damageAmount)
    {
        healthCurrent = Mathf.Clamp(healthCurrent - damageAmount, 0f, healthMax); // decrement health
        onHealthPercentChange?.Invoke(healthCurrent / healthMax); // update health
        if (healthCurrent <= 0f) // at zero health call death script
        {
            onDeath?.Invoke();
        }
    }
}