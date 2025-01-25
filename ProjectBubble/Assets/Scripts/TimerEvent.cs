using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimerEvent : MonoBehaviour
{
    [SerializeField] private float seconds = 0.2f;
    public UnityEvent OnTimeOut;

    private void OnEnable()
    {
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(seconds);

        OnTimeOut?.Invoke();
    }
}
