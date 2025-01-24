using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Interfaces currently not in use
public interface IDamageable
{
    void Damage(float amount);
}

public interface IHealable
{
    void Heal(float amount);
}

public class Health : Progressive //, IDamageable, IHealable
{
    //public UnityEvent onHealed;

    public UnityEvent onDamaged;
    
    public UnityEvent onDie;

    public float lastSubAmount { get; protected set; }

    // Decrease current value, not below zero
    public override void Sub(float amount)
    {
        float preChanged = Current;
        
        Current -= amount;

        lastSubAmount = amount;

        // Triggers the damage indicator
        // if the health value has changed
        if (preChanged != Current)
        {
            onDamaged?.Invoke();
        }

        if (Current <= 0)
        {
            Current = 0;
            StartCoroutine(SlowDie());
        }
    }

    /*
    // unnecessary
    //public override void Add(float amount)
    //{
    //    //float preChanged = Current;

    //    Current += amount;

    //    //if (preChanged != Current)
    //    //{
    //    //    onHealed?.Invoke();
    //    //}

    //    if (Current > Initial)
    //    {
    //        Current = Initial;
    //    }
    //}
    */

    // Trigger OnDie event after next frame
    protected IEnumerator SlowDie()
    {
        yield return null;
        onDie?.Invoke();
    }
}
