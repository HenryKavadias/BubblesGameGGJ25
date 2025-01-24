using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //public bool usePoolingSpawn = false;
    public GameObject prefab = null;

    protected GameObject assignedPrefab = null;
    private void Awake()
    {
        if (prefab && !assignedPrefab)
        {
            assignedPrefab = prefab;
        }
    }

    public void SpawnPassThrough(GameObject passedPrefab)
    {
        if (passedPrefab)
        {
            if (passedPrefab.GetComponent<PoolObject>())
            {
                PoolManager.Spawn(
                    passedPrefab.GetComponent<PoolObject>(),
                    transform.position, Quaternion.identity,
                    passedPrefab.transform.localScale);
                return;
            }
            else
            {
                // Default
                Instantiate(passedPrefab,
                    transform.position, Quaternion.identity);
                return;
            }
        }
        Debug.LogWarning("No prefab given...");
    }

    public void SpawnDefault()
    {
        if (assignedPrefab)
        {
            if (assignedPrefab.GetComponent<PoolObject>())
            {
                PoolManager.Spawn(
                    assignedPrefab.GetComponent<PoolObject>(),
                    transform.position, Quaternion.identity,
                    assignedPrefab.transform.localScale);
                return;
            }
            else
            {
                // Default
                Instantiate(assignedPrefab,
                    transform.position, Quaternion.identity);
                return;
            }
        }
        Debug.LogWarning("No prefab given...");
    }

    public void OverridePrefab(GameObject newPrefab)
    {
        if (newPrefab)
        { assignedPrefab = newPrefab; }
    }
}