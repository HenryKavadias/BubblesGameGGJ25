using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

// Controls the force and damage of explosive objects
public class BlastWave : MonoBehaviour
{
    public bool usePoolSystem = true;
    public bool enactOnStart = false;
    
    public int pointsCount = 100;   // Number of points drawn for the blast wave radius
    public float maxRadius = 50;    // Max radius of blast wave
    public float speed = 5;         // Speed of blast wave
    public float startWidth = 5;    // Starting width of blast wave

    public float damage = 10;

    public bool waveActive = true;
    public bool damageDropOff = true;

    [SerializeField] private float explosionForce = 40f;
    [SerializeField] private GameObject particleEffect;
    [SerializeField] private float particleEffectScale = 30;

    // Renderer for blast wave
    private LineRenderer lineRenderer;

    // Get the renderer and set the total number of draw points for it (needs one more than point count)
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = pointsCount + 1;

        lineRenderer.enabled = waveActive;
    }

    // Start Blast Wave
    private void Start()
    {
        if (!enactOnStart || 
            gameObject.GetComponent<PoolObject>()) 
        { return; }

        BeginBlast();
    }

    public void BeginBlast()
    {
        if (particleEffect)
        {
            // Any particle effect object parented to the root object must
            // have their scaling mode set to heirarchy
            if (particleEffect.GetComponent<PoolObject>())
            {

                if (particleEffect.TryGetComponent(out ParticleSystem ps))
                {
                    PoolManager.Spawn(
                        particleEffect?.GetComponent<PoolObject>(),
                        transform.position,
                        Quaternion.identity,
                        new Vector3(
                        particleEffectScale,
                        particleEffectScale,
                        particleEffectScale));
                }
                else if (particleEffect.TryGetComponent(out VisualEffect vfx))
                {
                    PoolObject obj = PoolManager.Spawn(
                        particleEffect?.GetComponent<PoolObject>(),
                        transform.position,
                        Quaternion.identity);

                    if (vfx.HasFloat("Scalar"))
                    {
                        obj.gameObject.GetComponent<VisualEffect>().
                            SetFloat("Scalar", particleEffectScale);
                    }

                }
            }
            else
            {
                // old system
                GameObject parEffect = Instantiate(particleEffect, transform.position, Quaternion.identity);

                parEffect.transform.localScale =
                    new Vector3(particleEffectScale, particleEffectScale, particleEffectScale);
            }
        }

        StartCoroutine(TriggerBlast());
    }

    // Apply damage and force to all objects hit by the blast wave
    private void Damage(float currentRadius)
    {
        // Get list of objects the explosion volume has hit 
        Collider[] hittingObjects = Physics.OverlapSphere(transform.position, currentRadius);

        for (int i = 0; i < hittingObjects.Length; i++)
        {
            // Don't interact with trigger colliders
            if (hittingObjects[i].isTrigger)
            {
                //Debug.Log("Trigger Collider");
                continue;
            }
            //Debug.Log(hittingObjects[i].gameObject.tag);
            if (hittingObjects[i].gameObject.tag == "Attack")
            {
                continue;
            }

            // Need to get the parent object with the rigid body.

            Transform objTransform = FindRidgidBody(hittingObjects[i].transform);

            if (objTransform == null)
            {
                continue;
            }

            Rigidbody rb = objTransform.gameObject.GetComponent<Rigidbody>();

            if (!rb)
            {
                //Debug.Log("No Force");
                continue;
            }

            // Adds force to the objects the blast hits (put the power function here)
            Vector3 direction = (hittingObjects[i].transform.position - transform.position).normalized;

            rb.AddForce(direction * explosionForce, ForceMode.Impulse);

            // Check if damageable then apply (fix this)
            Damageable damaged = objTransform.gameObject.GetComponent<Damageable>();

            if (!damaged)
            {
                continue;
            }

            // If damage value with drop off as radius of explosion gets larger
            if (damageDropOff)
            {
                damaged.Damage(damage / currentRadius);
            }
            else
            {
                damaged.Damage(damage);
            }

        }
    }

    // Search though an object to find its Rigidbody component in its object hierarchy
    private Transform FindRidgidBody(Transform transform)
    {
        Rigidbody currentObject = transform.gameObject.GetComponent<Rigidbody>();
        
        if (currentObject)
        {
            return currentObject.transform;
        }

        if (transform.parent != null)
        {
            return FindRidgidBody(transform.parent);
        }

        return null;
    }

    // Modifies the radius of the blast wave over time
    private IEnumerator TriggerBlast()
    {
        float currentRadius = 0f;

        while (currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed;

            if (waveActive)
            {
                Draw(currentRadius);
            }

            Damage(currentRadius);

            yield return null;
        }

        yield return null;
    }

    // Draws the visual effect of the blast wave with the line renderer
    private void Draw(float currentRadius)
    {
        float angleBetweenPoints = 360f / pointsCount;

        for (int i = 0; i <= pointsCount; i++)
        {
            float angle = i * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * currentRadius;

            lineRenderer.SetPosition(i, position);
        }

        lineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius / maxRadius);
    }
}
