using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private bool triggerOnlyOnce = false;
    private bool hasTriggeredOnce = false;
    private int collidersInZone = 0;

    [Header("EventoOnEnter")]
    public UnityEvent OnEnterEvent;

    [Header("EventoOnExit")]
    public UnityEvent OnExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;


        if (triggerOnlyOnce && hasTriggeredOnce) return;

        collidersInZone++;

        if (collidersInZone == 1)
        {
            OnEnterEvent.Invoke();
            hasTriggeredOnce = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        collidersInZone--;
        if (collidersInZone < 0) collidersInZone = 0;

        if (collidersInZone == 0)
        {
            OnExitEvent.Invoke();
        }

    }
}
