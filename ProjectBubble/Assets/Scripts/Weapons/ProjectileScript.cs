using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class ProjectileScript : Damager
{
    public float BulletLife = 5f;
    protected float BulletLifeTimer = 0;
    public float DamagePowerUpMultiplier = 1f;
    public LayerMask Collisions;
    public float armoredModifier = 1.5f;
    public float unarmoredModifier = 0.5f;
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
            DamageEntity(entity.transform.root.gameObject);
            OnHit?.Invoke();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (KillProjectile(other))
        {
            DamageEntity(other.transform.root.gameObject);
            OnHit?.Invoke();
        }
    }

    private void DamageEntity(GameObject entity)
    {
        Damageable damageable =
            entity.GetComponent<Damageable>();

        if (damageable)
        {
            if (entity.TryGetComponent(out EnemyController ec))
            {
                bool arm = ec.ArmorIsUp;

                if (arm) 
                {
                    //Debug.Log("Armored");
                    damageable.Damage(damage * armoredModifier);
                }
                else
                {
                    //Debug.Log("unamored");
                    damageable.Damage(damage * unarmoredModifier);
                }
            }
            else
            {
                //Debug.Log("normal");
                damageable.Damage(damage);
            }
        }
        else
        {
            Debug.Log("Has no damageable");
        }
    }

    private bool KillProjectile(Collider other)
    {
        return other.gameObject.layer == LayerMask.NameToLayer("Wall")
               || other.gameObject.layer == LayerMask.NameToLayer("Ground") ||
               other.gameObject.layer == LayerMask.NameToLayer("Enemy") ||
               other.gameObject.layer == LayerMask.NameToLayer("Player");
    }
    protected Rigidbody _rb => GetComponent<Rigidbody>();
    public Rigidbody RB => _rb;
}
