using UnityEngine;
using UnityEngine.Events;

public class CamaraTriggerZone : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private bool triggerOnlyOnce = false;
    private bool hasTriggered = false;
    private bool hasTriggeredOnce = false;

    [Header("EventoOnEnter")]
    public UnityEvent OnEnterEvent;

    [Header("EventoOnExit")]
    public UnityEvent OnExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(targetTag)) return;


        if(triggerOnlyOnce && hasTriggeredOnce) return;

        if(!hasTriggered)
        {
            OnEnterEvent.Invoke();
            hasTriggered = true;
            hasTriggeredOnce = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        OnExitEvent.Invoke();

        hasTriggered = false;
    }
}
