using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ProjectileScript : Damager
{
    public float BulletLife = 5f;
    protected float BulletLifeTimer = 0;
    public float DamagePowerUpMultiplier = 1f;
    public LayerMask Collisions;
    //protected virtual float RandomWeaponDamage => Random.Range(damageMin, damageMax);
    private Vector3 _previousPos;

    public UnityEvent OnHit;

    private void OnEnable()
    { BulletLifeTimer = BulletLife; }

    private void Update()
    {
        Vector3 toPosition = transform.position;
        Vector3 direction = (toPosition - _previousPos).normalized;

        if (Physics.Raycast(_previousPos, direction, out RaycastHit hit,Vector3.Distance(_previousPos, toPosition), Collisions,
                QueryTriggerInteraction.Collide))
        {
            HitEntity(hit.transform.gameObject);
        }

        if (BulletLifeTimer > 0)
        { BulletLifeTimer -= Time.deltaTime; }
        else
        {
            OnHit?.Invoke();
        }
    }

    private void LateUpdate()
    {
        _previousPos = transform.position;
    }
    void HitEntity(GameObject entity)
    {
        if (KillProjectile(entity.GetComponent<Collider>()))
        {
            OnHit?.Invoke();
            //Destroy(gameObject);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (KillProjectile(other))
        {
            Damageable damageable =
            other.transform.root.GetComponent<Damageable>();

            if (damageable)
            { Damage(damageable); }
            OnHit?.Invoke();
        }
    }

    private bool KillProjectile(Collider other)
    {
        return other.gameObject.layer == LayerMask.NameToLayer("Wall")
               || other.gameObject.layer == LayerMask.NameToLayer("Ground") ||
               other.gameObject.layer == LayerMask.NameToLayer("Enemy");
    }
    protected Rigidbody _rb => GetComponent<Rigidbody>();
    public Rigidbody RB => _rb;
}
