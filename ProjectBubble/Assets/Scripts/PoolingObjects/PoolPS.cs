using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(ParticleSystem))]
public class PoolPS : PoolObject
{
    private ParticleSystem ps;
    private void Awake() => ps = GetComponent<ParticleSystem>();

    /// <summary>
    /// Active state of the object
    /// </summary>
    public override bool Active
    {
        get => ps.isPlaying;

        protected set
        {
            if (value)
                ps.Play();
            else
                ps.Stop();
        }
    }
}