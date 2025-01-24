using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// Warning, if this object has and post death processes, DON'T use destroythis script
// on the object until those process is finished.
// (possibly use destory by time, or just deactivate the game object)
public class Damageable : MonoBehaviour
{
    [SerializeField, Required] protected Progressive _health;
    [SerializeField] protected Shield _shield = null;

    // makes the object temporarily invincible on spawn
    [SerializeField] protected bool invincible = true;
    [SerializeField] protected float invincibleTime = 0.2f;

    public UnityEvent hitEvent;

    private void Start()
    {
        if (_health == null)
        { _health = gameObject.GetComponent<Progressive>(); }

        Invoke(nameof(TurnOffInvincible), invincibleTime);
    }

    public void TriggerInvinvible(float time = 0f)
    {
        invincible = true;

        if (time > 0f)
        {
            Invoke(nameof(TurnOffInvincible), time);

            return;
        }

        Invoke(nameof(TurnOffInvincible), invincibleTime);
    }

    protected void TurnOffInvincible()
    { invincible = false; }

    // Damage object
    public virtual void Damage(
        float damage, GameObject attacker = null)
    {
        hitEvent.Invoke();
        // prevents overlapping hit ties
        if (_health.Current <= 0 || invincible)
        {
            return;
        }
        if (_shield && _shield.Current > 0)
        {
            _shield.Sub(damage);
            return;
        }

        _health.Sub(damage);
    }

    // Heal object
    public virtual void Heal(float heal)
    {
        _health.Add(heal);
    }

}
