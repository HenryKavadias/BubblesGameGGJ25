using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TriggerToggle : MonoBehaviour
{
    [SerializeField]
    internal bool ActiveTrigger = true;
 
    [SerializeField]
    internal bool DeactivateOnActivation = false;

    private BoxCollider bc => GetComponent<BoxCollider>();
    
    public UnityEngine.Events.UnityEvent ReactionEvent;
    
    [SerializeField] private bool TriggerActive;

    [SerializeField] private List<Transform> TriggerEnable = new();

    [SerializeField] private List<Transform> TriggerDisable = new();


    public bool NewMusicTrack = false;
    public bool NewAmbienceTrack = false;
    
           
    private void OnTriggerEnter(Collider other)
    {
        if (!ActiveTrigger)
            return;
        if (!other.CompareTag("Player"))
            return;
        
        if (ReactionEvent.GetPersistentEventCount() > 0)
            ReactionEvent.Invoke();
        
        TriggerEnableCheck();
        TriggerDisableCheck();
        
        
        gameObject.SetActive(DeactivateOnActivation);
    }
    
    void TriggerEnableCheck()
    {
        if (TriggerEnable.Count <= 0) return;
        foreach (Transform a in TriggerEnable)
        {
            a.gameObject.SetActive(true);
            if (a.GetComponent<TriggerToggle>())
            {
                a.GetComponent<TriggerToggle>().ActiveTrigger = true;
            }
        }
    }
    void TriggerDisableCheck()
    {
        if (TriggerDisable.Count <= 0) return;
        foreach (Transform a in TriggerDisable)
        {
            a.gameObject.SetActive(false);
            if (a.GetComponent<TriggerToggle>())
            {
                a.GetComponent<TriggerToggle>().ActiveTrigger = false;
            }
        }
    }
}