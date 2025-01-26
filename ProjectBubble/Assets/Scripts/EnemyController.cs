using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Analytics;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navAgentComponent;
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

    private void Start()
    {
        ArmorIsUp = startWithArmor;
    }

    public void DisableArmor()
    { ArmorIsUp = false; }

    public void HasBeenKilled()
    {
        bodyVisaul.SetActive(false);
        Destroy(gameObject, 1f);
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

            ProjectileScript projectile = atk.GetComponent<ProjectileScript>();

            projectile.RB.linearVelocity = attackSpawn.forward * rangedProjectileSpeed;

            OnRangedAttack?.Invoke();
        }
    }

    private void DoMeleeAttack()
    {
        if (!meleeAttack) { return; }

        PoolObject atk;

        if (meleeAttack.TryGetComponent(out PoolObject pref))
        {
            atk = PoolManager.Spawn(pref, attackSpawn.position,
                Quaternion.identity);


            OnMeleeAttack?.Invoke();
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
        if (!target) { return; }
        navAgentComponent.destination = target.transform.position;
    }

 
}
