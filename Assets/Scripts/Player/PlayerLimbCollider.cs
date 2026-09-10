using UnityEngine;

public class PlayerLimbCollider : MonoBehaviour
{
    private PlayerImpactHandler m_Handler;

    private void Awake()
    {
        m_Handler = GetComponentInParent<PlayerImpactHandler>();
        Debug.Log("Encontro playerHandler");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_Handler == null) return;

        m_Handler.ProcessCollision(collision);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (m_Handler == null) return;

        m_Handler.ProcessTrigger(other);
    }
}
