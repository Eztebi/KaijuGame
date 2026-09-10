
using UnityEngine;
using UnityEngine.Events;

public class PlayerImpactHandler : MonoBehaviour
{
    [Header("Configuración de Impacto")]
    [SerializeField] private string bulletTag = "Bullet";
    [SerializeField] private string hazardTag = "Hazard";
    [SerializeField] private float minVelocityToRagdoll = 12f;

    [Header("Referencias")]
    [SerializeField] private InputHandler inputHandler;

    public UnityEvent OnPlayerHit;
    public UnityEvent OnEnterRagdoll;

    public void ProcessCollision(Collision collision)
    {
        if (!collision.collider.CompareTag(bulletTag)) return;
        
        OnPlayerHit.Invoke();

        if (collision.relativeVelocity.magnitude >= minVelocityToRagdoll)
        {
            ForceRagdollState();
        }
    }

    public void ProcessTrigger(Collider other)
    {
        if (other.CompareTag(hazardTag))
        {
            ForceRagdollState();
        }
    }
    public void ForceRagdollState()
    {
        if (inputHandler == null) return;

        inputHandler.BodyActiveState = InputHandler.EBodyActiveState.Ragdoll;
        inputHandler.LeftLegActiveState = InputHandler.EBodyActiveState.Ragdoll;
        inputHandler.RightLegActiveState = InputHandler.EBodyActiveState.Ragdoll;

        OnEnterRagdoll.Invoke();
    }
}
