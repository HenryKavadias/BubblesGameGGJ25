using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileScript : MonoBehaviour
{
    public float BulletLife = 15f;
    protected float BulletLifeTimer = 0;
    public float damageMin = 0, damageMax = 0;
    public float DamagePowerUpMultiplier = 1f;
    public LayerMask Collisions;
    protected virtual float RandomWeaponDamage => Random.Range(damageMin, damageMax);
    private Vector3 _previousPos;

    private void Update()
    {
        Vector3 toPosition = transform.position;
        Vector3 direction = (toPosition - _previousPos).normalized;

        if (Physics.Raycast(_previousPos, direction, out RaycastHit hit,Vector3.Distance(_previousPos, toPosition), Collisions,
                QueryTriggerInteraction.Collide))
        {
            HitEntity(hit.transform.gameObject);
        }
    }

    private void LateUpdate()
    {
        _previousPos = transform.position;
    }
    void HitEntity(GameObject entity)
    {
        if (gameObject.CompareTag("Enemy"))
        {
            
        }
        if (KillProjectile(entity.GetComponent<Collider>()))
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (KillProjectile(other))
        {
            Destroy(gameObject);
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
