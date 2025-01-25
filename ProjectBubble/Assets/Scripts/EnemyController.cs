using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navAgentComponent;
    [SerializeField] private float aggressionRange = 1000f;
    private GameObject target;

    private void Update()
    {
        FindTarget();

        SetDestination();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.CompareTag("Projectile"))
        { Debug.Log("Hit"); }
    }

    private bool IsWithinDistance(Vector3 position, float distance)
    { return Vector3.Distance(transform.position, position) <= distance; }

    private void FindTarget()
    {
        if (target) { return; }
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && IsWithinDistance(player.transform.position, aggressionRange))
        {
            target = player;
        }
    }

    private void SetDestination()
    {
        if (!target) { return; }
        navAgentComponent.destination = target.transform.position;
    }
}
