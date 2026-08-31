using UnityEngine;
using PrimeTween;
using Unity.Cinemachine;
public class FeetCollisionMessenger : MonoBehaviour
{
    [SerializeField] private ParticleSystem _rumbleVFX;
    [SerializeField] private Camera _camera;
    public LegStateMachine stateMachine;

    private void OnCollisionEnter(Collision collision)
    {
        if (stateMachine != null)
        {
            Debug.Log("CHOCOOOOOOOOOOOOOOOOOOOOOOOOO");
            stateMachine.OnCollisionEnter(collision);
            Tween.ShakeCamera(_camera, strengthFactor: 1.0f, duration: 0.5f, frequency: 10);
            _rumbleVFX.Play();
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