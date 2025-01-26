using UnityEngine;
using UnityEngine.Events;

public class TriggerVolume : MonoBehaviour
{
    public bool activeOnStart;

    private bool isActive;

    public UnityEvent OnTriggered;

    private void Start()
    {
        isActive = activeOnStart;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) { return; }

        isActive = false;

        OnTriggered?.Invoke();
    }
}
