using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shield : Health
{
    [SerializeField] private bool canRegen = true;
    [SerializeField] private float regenAmount;
    [SerializeField] private float regenRate;
    [SerializeField] private float regenDelay; // after being damaged
    [SerializeField] private float brokenDelay;
    [SerializeField] private float regenHangTime = 3f;

    [SerializeField] private SkinnedMeshRenderer meshRenderer, UnArmoured;
    [SerializeField] private Collider colliderShell;

    private float rateCooldown = 0;
    private float delayCooldown = 0;

    public UnityEvent StartRegen;

    protected bool playingRegenAnimation = false;

    protected enum shieldState
    {
        idle,
        regen,
        delayed,
        broken,
        destroyed
    }
    
    protected shieldState state;

    public void SetAsDestroyed()
    { 
        SetState(shieldState.destroyed);
        playingRegenAnimation = false;
    }

    public override void Sub(float amount)
    {
        if (amount <= 0) { return; }

        if (state == shieldState.broken)
        { SetState(shieldState.broken); } 
        else
        { SetState(shieldState.delayed); }

        float preChanged = Current;

        Current -= amount;

        lastSubAmount = amount;

        // Triggers the damage indicator
        // if the health value has changed
        if (preChanged != Current)
        {
            onDamaged?.Invoke();
        }

        playingRegenAnimation = false;

        if (Current <= 0)
        {
            colliderShell.enabled = false;
            meshRenderer.enabled = false;
            UnArmoured.enabled = true;
            SetState(shieldState.broken);
            Current = 0;
            StartCoroutine(SlowDie());
        }
    }

    public float GetRegenTime()
    {
        return (((Initial - Current) * regenRate) / regenAmount) + regenHangTime;
    }

    protected void Update()
    {
        if (!canRegen) { return; }

        if (state == shieldState.idle || 
            state == shieldState.destroyed)
        { return; }

        if (state == shieldState.regen)
        { 
            if (rateCooldown > 0)
            {
                rateCooldown -= Time.deltaTime;
                return;
            }
            
            if (playingRegenAnimation == false)
            {
                playingRegenAnimation = true;
                //Debug.Log(GetRegenTime());
                StartRegen?.Invoke();
            }

            Add(regenAmount);

            if (Current >= Initial)
            {
                SetState(shieldState.idle);
                playingRegenAnimation = false;
                return;
            }

            rateCooldown = regenRate;
        }
        else if (state == shieldState.delayed || 
            state == shieldState.broken)
        {
            if (delayCooldown > 0)
            {
                delayCooldown -= Time.deltaTime;
                return;
            }
            SetState(shieldState.regen);
        }
    }

    protected void SetState(shieldState _state)
    {
        switch (_state)
        {
            case shieldState.regen:
                rateCooldown = regenRate;
                break;
            case shieldState.delayed:
                delayCooldown = regenDelay;
                break;
            case shieldState.broken:
                delayCooldown = brokenDelay;
                break;
            default:
                break;
        }
        if (state != _state)
        { state = _state; }
    }
}
