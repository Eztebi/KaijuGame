using UnityEngine;
using UnityEngine.Events;

public class CamaraTriggerZone : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private bool triggerOnlyOnce = false;
    private bool hasTriggered = false;

    [Header("EventoOnEnter")]
    public UnityEvent OnEnterEvent;

    [Header("EventoOnExit")]
    public UnityEvent OnExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && !hasTriggered) return;

        OnEnterEvent.Invoke();
        hasTriggered = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag) && !hasTriggered) return;

        OnEnterEvent.Invoke();
        hasTriggered = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag) && triggerOnlyOnce)
        {
            hasTriggered = true;
        }
        else if (other.CompareTag(targetTag))
        {
            OnExitEvent.Invoke();
            hasTriggered = false;
        }
    }
}
