using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damager : MonoBehaviour
{
    public float damageAmount;
    [SerializeField] UnityEvent onDamage;

    // On collision with damagable, cause damage
    public void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.Damage(damageAmount);
            onDamage?.Invoke();
        }
    }
}
