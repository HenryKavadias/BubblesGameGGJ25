using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damager : MonoBehaviour
{
    public float damage = 1;    // Amount of unmodified damage

    // Apply damage to the relative damageable gameobject
    public virtual void Damage(Damageable damageable) => damageable.Damage(damage);

    public virtual void SetDamage(float dam = 1f)
    {
        damage = dam;
    }
    // pending for use
    //protected void ApplyDamage(IDamageable damageable) => damageable.Damage(damage);
}
