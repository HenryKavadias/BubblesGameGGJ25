using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDamager : Damager
{
    public UnityEvent onContact;

    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.transform.root.GetComponent<Damageable>();

        if (damageable)
        {
            damageable.Damage(damage);
        }
        Debug.Log("hit");
        onContact.Invoke();
    }
}
