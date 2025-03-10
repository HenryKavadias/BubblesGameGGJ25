using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gibbing : MonoBehaviour
{
    [SerializeField]
    private Giblet[] GibsList;

    [SerializeField]
    private float forceMin = 10, forceMax = 20;
    [SerializeField]
    private float extraForceMin = 1.5f, extraForceMax = 2;

    private Transform head;

    private bool _gibbed;

    public void StartGib()
    {
       Debug.Log("StartGib");
        //Prevent multiple Gib calls
        if (_gibbed) return;
        _gibbed = true;
        foreach (Giblet gib in GibsList)
        {
            if (!gib.canSpawn)
                break;
            
            // for (int i = 0; i < (!gib.OverrideAmount ? gib.AmountAuto : gib.Amount); i++)
            for (int i = 0; i < (gib.Part.SpawnLocation.Count); i++)
            {
                if (gib.OverrideAmount)
                {
                    for (int ii = 0; ii < gib.Amount; ii++)
                        GibInstantiate(gib, i);

                }
                else
                    GibInstantiate(gib, i);
            }
        }
    }

    private void GibInstantiate(Giblet gib, int i)
    {
        GameObject giblet = 
            Instantiate(gib.Part.Chunk, 
                gib.Part.SpawnLocation.Count > 0 ? gib.Part.SpawnLocation[i].transform.position : transform.position, 
                gib.Part.SpawnLocation.Count > 0 ? gib.Part.SpawnLocation[i].transform.rotation : transform.rotation);

        if (giblet.TryGetComponent(out DespawnProp a))
        {
            a.DoNotDisappear = !gib.Despawn;
            a.Lifespan = gib.DespawnTimer;
        }
        giblet.transform.localScale = Vector3.one * gib.PartScale;
        Vector3 dir = Random.insideUnitCircle;
        dir.y = Random.Range(-0.1f, 1);

        giblet.GetComponent<Rigidbody>().linearVelocity = dir * Random.Range(forceMin, forceMax);
    }
    
}

[System.Serializable]
public class Giblet
{
    public string BodyPart;
    public GibPrefab Part;
    public bool OverrideAmount = false;
    public int Amount = 0;
    public int AmountAuto => Part.SpawnLocation.Count;
    public bool canSpawn = true;
    public float PartScale = 1;
    public bool Despawn = false;
    public float DespawnTimer = 30;
}

[System.Serializable]
public class GibPrefab
{
    public GameObject Chunk;
    public List<GameObject> SpawnLocation = new List<GameObject>();
}

[System.Serializable]
public class GibbingSpawnIndex : UnitySerializedDictionary<GameObject, int>
{}

public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField, HideInInspector]
    private List<TKey> keyData = new List<TKey>();

    [SerializeField, HideInInspector]
    private List<TValue> valueData = new List<TValue>();

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        Clear();
        for (int i = 0; i < this.keyData.Count && i < this.valueData.Count; i++)
            this[this.keyData[i]] = this.valueData[i];

    }
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        keyData.Clear();
        valueData.Clear();

        foreach (var item in this)
        {
            keyData.Add(item.Key);
            valueData.Add(item.Value);
        }
    }
}
