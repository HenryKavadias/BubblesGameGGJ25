using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class PoolVFX : PoolObject
{
    private VisualEffect vfx;
    private float lifeTime;
    private void Awake()
    {
        vfx = GetComponent<VisualEffect>();

        lifeTime = 3f;
        if (vfx.HasFloat("MaxLifetime"))
        { lifeTime = vfx.GetFloat("MaxLifetime"); }
        else 
        {
            Debug.Log("Can't find 'MaxLifetime' property");
            //vfx.SetFloat("MaxLifetime", lifeTime);
        }
    }

    private IEnumerator RunLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Active = false;
    }
    /// <summary>
    /// Active state of the object
    /// </summary>
    public override bool Active
    {
        get { return gameObject.activeSelf; }

        protected set
        {  
            if (value)
            {
                gameObject.SetActive(value);
                vfx.Play();
                StartCoroutine(RunLifeTime());
            }  
            else
            {
                vfx.Stop();
                gameObject.SetActive(value);
            }
        }
    }
    // This method is ineffective, keeps reusing vfx that hadn't played yet
    /*
    private VisualEffect vfx;
    private void Awake() => vfx = GetComponent<VisualEffect>();

    /// <summary>
    /// Active state of the object
    /// </summary>
    public override bool Active
    {
        get
        { return vfx.HasAnySystemAwake(); }

        protected set
        {
            if (value)
                vfx.Play();
            else
                vfx.Stop();
        }
    }
    */
}
