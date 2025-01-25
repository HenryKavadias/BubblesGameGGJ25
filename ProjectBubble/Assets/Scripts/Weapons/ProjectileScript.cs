using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField]
    private WeaponParent _weapon;
    public float BulletLife = 15f;
    protected float BulletLifeTimer = 0;
    public float damageMin = 0, damageMax = 0;
    public float DamagePowerUpMultiplier = 1f;
    protected virtual float RandomWeaponDamage => Random.Range(damageMin, damageMax);
    protected Rigidbody _rb => GetComponent<Rigidbody>();
    public Rigidbody RB => _rb;
    
    public WeaponParent Weapon { get => _weapon; set => _weapon = value; }
}
