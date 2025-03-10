
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounterController : MonoBehaviour
{
    [SerializeField] private List<EnemyController> activeEnemies = new();
    public UnityEngine.Events.UnityEvent ReactionEvent;

    void Start()
    {
        foreach (var child in activeEnemies)
        {
            child.Director = this;
        }
    }
    public void EnemyPerished(EnemyController son)
    {
        if (activeEnemies.Contains(son))
        {
            activeEnemies.Remove(son);
        }

        CheckAllDead();
    }
    

    void CheckAllDead()
    {
        if (activeEnemies.Count <= 0)
        {
            ReactionEvent.Invoke();
        }
    }
}
