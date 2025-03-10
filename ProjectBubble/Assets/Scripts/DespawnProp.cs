using UnityEngine;
public class DespawnProp : MonoBehaviour
{
    public float Lifespan = 30, DisappearSpeed = 1;
    public float LifespanTimer = 0;

    [SerializeField] private ParticleSystem[] PS;
    public bool DoNotDisappear = true;

    protected virtual void Start()
    {
        OnPoolRetrieve();
    }

    private void OnPoolRetrieve()
    {
        LifespanTimer = 0;
        foreach (var particle in PS)
        {
            particle.gameObject.SetActive(true);
            particle.Play();
        }
    }

    void Update()
    {
        if (DoNotDisappear)
            return;

        if (LifespanTimer < Lifespan)
        {
            LifespanTimer += Time.deltaTime;
            return;
        }

        if (transform.localScale.sqrMagnitude > 0)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime * DisappearSpeed);
            return;
        }
        gameObject.SetActive(false);
    }
}
