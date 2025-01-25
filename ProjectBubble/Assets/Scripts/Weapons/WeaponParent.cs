using UnityEngine;

public abstract class WeaponParent : MonoBehaviour
{
   [SerializeField] private int _ammoUsage = 1;
   [SerializeField] private float _maxClipSize = 14f;
   [SerializeField] private float _reloadCurrentTimerMax = 2;
   [SerializeField] private float _damageMin,_damageMax;
   [SerializeField] private GameObject _projectilePrefab;

   [SerializeField] protected float _spread;
   protected float _spreadAngle = 360f;
   public float FireRate = 1f;
   protected float nextTimeToFire = 0f;
   protected Vector3 gunDir, forwardVector, hitSpot;
   private float _reloadCurrentTimer = 0;
   public GameObject fpsCam { get; set; }
   protected RaycastHit weaponhit;
   [SerializeField] private int ProjectileSpeed = 250;
   
   public Transform ShootPoint; //, Barrel2;
   public Transform AgainstWallShootPoint; //, Barrel2Actual;
   internal Transform shootPointRef;
   public LayerMask CollisionCheck = 1 << 7| 1 << 9 | 1 << 13 | 1 << 14 | 1 << 15 | 1 << 19; //Look for walls, ceiling or the ground

   [SerializeField] private int RaycastRange = 100000;
   protected InputDetector shootInput = new ();
   public void HandlePlayerInputs(bool shoot)
   {
      shootInput.inputState = shoot;
   }
   private void Awake()
   {
      //ComponentCacheIndex.WeaponEntities.Add(transform, this);
      shootPointRef = ShootPoint;
   }


   public virtual void Shoot()
   {
      
      //if (Time.time >= nextTimeToFire) // || !WeaponAnimation[0].isPlaying && AttackBuffer == true)
      //{
         ShotDirection(_spread, _spreadAngle);
         ShootProjectile(_projectilePrefab);
         
      //}
   }

   protected virtual void ShotDirection(float spread, float spreadAngle)
   {
      forwardVector = Vector3.forward;

      float deviation = Random.Range(0f, spread);
      float angle = Random.Range(0f, spreadAngle);
      forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
      forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
      forwardVector = fpsCam.transform.rotation * forwardVector;

      if (Physics.Raycast(fpsCam.transform.position, forwardVector, out weaponhit, RaycastRange, CollisionCheck, QueryTriggerInteraction.Collide))
      {
            gunDir = (weaponhit.point - shootPointRef.position).normalized;
            hitSpot = weaponhit.point;
      }
      else
      {
         gunDir = forwardVector;
      }
   }
   
   protected virtual void ShootProjectile(GameObject prefab)
   {
        //GameObject a = Instantiate(prefab, shootPointRef.position, fpsCam.transform.rotation);
      PoolObject atk;
      if (prefab.TryGetComponent(out PoolObject pref))
      {
            atk = PoolManager.Spawn(pref, shootPointRef.position, fpsCam.transform.rotation);
      }
      else
      {
          return;
      }
      
            
      ProjectileScript projectile = atk.GetComponent<ProjectileScript>();
      
      //Debug.Log(projectile.RB.linearVelocity);
      _projectilePrefab.transform.position = shootPointRef.position + (gunDir);

      projectile.RB.linearVelocity = gunDir * ProjectileSpeed;
   }
   
   private void OnCollisionExit(Collision other)
   {
      if (other.gameObject.transform.gameObject.layer == LayerMask.NameToLayer("Wall")|| other.gameObject.CompareTag("Enemy"))
      {
         shootPointRef = ShootPoint;
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.gameObject.transform.gameObject.layer == LayerMask.NameToLayer("Wall") || other.gameObject.CompareTag("Enemy"))
      {
         shootPointRef = ShootPoint;
      }
   }
}
