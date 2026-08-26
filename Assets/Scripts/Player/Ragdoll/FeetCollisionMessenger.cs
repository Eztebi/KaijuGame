using UnityEngine;

public class FeetCollisionMessenger : MonoBehaviour
{
    public LegStateMachine stateMachine;

    private void OnCollisionEnter(Collision collision)
    {
        // ESTE ES EL NUEVO DEBUG:

        if (stateMachine != null)
        {
            Debug.Log("CHOCOOOOOOOOOOOOOOOOOOOOOOOOO");
            stateMachine.OnCollisionEnter(collision);
        }
        else
        {
            Debug.LogError("MESSENGER ERROR: " + gameObject.name + " no tiene StateMachine asignada.");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (stateMachine != null)
        {
            stateMachine.OnCollisionStay(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (stateMachine != null)
        {
            stateMachine.OnCollisionExit(collision);
        }
    }
}