using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public class EnemyController : MonoBehaviour
{
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int Punch = Animator.StringToHash("Punch");
    [SerializeField] private NavMeshAgent navAgentComponent;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator noArmourAnimator;
    [SerializeField] private float aggressionRange = 1000f;
    private GameObject target;
    [SerializeField] private bool startWithArmor = false;
    [SerializeField] private GameObject meleeAttack;
    [SerializeField] private GameObject rangedAttack;
    [SerializeField] private Transform attackSpawn;
    [SerializeField, Required] private GameObject bodyVisaul;

    [SerializeField] private float rangedProjectileSpeed = 5;

    [SerializeField] private float attackSpeed = 2;
    private float attackCooldown;

    public UnityEvent OnMeleeAttack;
    public UnityEvent OnRangedAttack;

    public bool ArmorIsUp { get; private set; } = false;
    public Transform VFX;
    [SerializeField] private Transform VFXPoint;
    [SerializeField] private float _gibbingTimerMax = 1.25f;
    [SerializeField] private float _explosionTargetMax = 1;

    private float _explosionTarget= 0;
    public void SpawnVFX()
    {
        Vector3 pos = transform.position;
        pos.y -=1;
        Instantiate(VFX, VFXPoint.position, transform.rotation);
        navAgentComponent.enabled = false;
        target = null;
        //bodyVisaul.SetActive(false);
        Invoke("GibEnemy", _gibbingTimerMax);
    }
    
    private void Start()
    {
        ArmorIsUp = startWithArmor;
    }

    public void DisableArmor()
    { ArmorIsUp = false; }

    public void HasBeenKilled()
    {

        Invoke("DestroyEnemy", _explosionTargetMax);
        
    }

    void GibEnemy()
    {
        if (!GetComponent<Gibbing>())
            return;
        GetComponent<Gibbing>().StartGib();
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        FindTarget();

        SetDestination();

        AttackBehavour();
    }

    private void AttackBehavour()
    {
        if (!target) { return; }

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
            return;
        }

        if (Vector3.Distance(transform.position, 
            target.transform.position) <= navAgentComponent.stoppingDistance) 
        {
            DoMeleeAttack();
        }
        else
        {
            DoRangedAttack();
        }

        attackCooldown = attackSpeed;
    }

    private void DoRangedAttack()
    {
        if (!rangedAttack) { return; }

        PoolObject atk;

        if (rangedAttack.TryGetComponent(out PoolObject pref))
        {
            atk = PoolManager.Spawn(pref, attackSpawn.position, 
                Quaternion.LookRotation(target.transform.position));
            animator.SetBool(Punch, true);
            ProjectileScript projectile = atk.GetComponent<ProjectileScript>();

            projectile.RB.linearVelocity = attackSpawn.forward * rangedProjectileSpeed;

            OnRangedAttack?.Invoke();
            animator.SetBool(Punch, false);
        }
    }

    private void DoMeleeAttack()
    {
        if (!meleeAttack) { return; }

        PoolObject atk;
        
        if (meleeAttack.TryGetComponent(out PoolObject pref))
        {
            noArmourAnimator.SetBool(Punch, true);
            atk = PoolManager.Spawn(pref, attackSpawn.position,
                Quaternion.identity);


            OnMeleeAttack?.Invoke();
            noArmourAnimator.SetBool(Punch, false);
        }
    }

    private bool IsWithinDistance(Vector3 position, float distance)
    { return Vector3.Distance(transform.position, position) <= distance; }

    private void FindTarget()
    {
        if (target) { return; }
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && IsWithinDistance(player.transform.position, aggressionRange))
        {
            attackCooldown = attackSpeed;
            target = player;
        }
    }

    private void SetDestination()
    {
        animator.SetBool(Moving,navAgentComponent.velocity.magnitude>0);
        noArmourAnimator.SetBool(Moving,navAgentComponent.velocity.magnitude>0);
        if (!target) { return; }
        navAgentComponent.destination = target.transform.position;
    }

 
}
